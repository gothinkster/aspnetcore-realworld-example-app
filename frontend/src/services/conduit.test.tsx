import axios, { AxiosStatic } from 'axios';
import {
  createArticle,
  createComment,
  deleteArticle,
  deleteComment,
  favoriteArticle,
  followUser,
  getArticle,
  getArticleComments,
  getArticles,
  getFeed,
  getProfile,
  getTags,
  getUser,
  login,
  signUp,
  unfavoriteArticle,
  unfollowUser,
  updateArticle,
  updateSettings,
} from './conduit';

jest.mock('axios');

const mockedAxios = axios as jest.Mocked<AxiosStatic>;

const defaultArticle = {
  slug: 'how-to-train-your-dragon',
  title: 'How to train your dragon',
  description: 'Ever wonder how?',
  body: 'It takes a Jacobian',
  tagList: ['dragons', 'training'],
  createdAt: '2016-02-18T03:22:56.637Z',
  updatedAt: '2016-02-18T03:48:35.824Z',
  favorited: false,
  favoritesCount: 0,
  author: {
    username: 'jake',
    bio: 'I work at statefarm',
    image: 'https://i.stack.imgur.com/xHWG8.jpg',
    following: false,
  },
};

const defaultComment = {
  id: 1,
  createdAt: '2016-02-18T03:22:56.637Z',
  updatedAt: '2016-02-18T03:22:56.637Z',
  body: 'It takes a Jacobian',
  author: {
    username: 'jake',
    bio: 'I work at statefarm',
    image: 'https://i.stack.imgur.com/xHWG8.jpg',
    following: false,
  },
};

it('Should get articles', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      articles: [
        defaultArticle,
        {
          slug: 'how-to-train-your-dragon-2',
          title: 'How to train your dragon 2',
          description: 'So toothless',
          body: 'It a dragon',
          tagList: ['dragons', 'training'],
          createdAt: '2016-02-18T03:22:56.637Z',
          updatedAt: '2016-02-18T03:48:35.824Z',
          favorited: false,
          favoritesCount: 0,
          author: {
            username: 'jake',
            bio: 'I work at statefarm',
            image: 'https://i.stack.imgur.com/xHWG8.jpg',
            following: false,
          },
        },
      ],
      articlesCount: 2,
    },
  });

  const result = await getArticles();
  expect(result.articles.length).toBe(2);
  expect(result.articlesCount).toBe(2);
});

it('Should get tags', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      tags: ['reactjs', 'angularjs'],
    },
  });

  const result = await getTags();
  expect(result.tags.length).toBe(2);
  expect(result.tags).toContain('angularjs');
});

it('Should send correct login object', async () => {
  mockedAxios.post.mockRejectedValueOnce({ response: { data: { errors: { x: ['y', 'z'] } } } });

  await login('thisIsUser', 'thisIsPassword');

  const call = mockedAxios.post.mock.calls[0];

  expect(call[1]).toHaveProperty('user');
  expect(call[1].user).toHaveProperty('email', 'thisIsUser');
  expect(call[1].user).toHaveProperty('password', 'thisIsPassword');
});

it('Should get login errors', async () => {
  mockedAxios.post.mockRejectedValueOnce({ response: { data: { errors: { x: ['y', 'z'] } } } });

  const result = await login('', '');
  result.match({
    ok: () => fail(),
    err: (e) => {
      expect(e).toHaveProperty('x');
      expect(e['x']).toHaveLength(2);
    },
  });
});

it('Should get user on successful login', async () => {
  mockedAxios.post.mockResolvedValueOnce({
    data: {
      user: {
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      },
    },
  });

  const result = await login('', '');
  result.match({
    ok: (user) => {
      expect(user).toHaveProperty('email', 'jake@jake.jake');
      expect(user).toHaveProperty('token', 'jwt.token.here');
    },
    err: () => fail(),
  });
});

it('Should return article on favorite', async () => {
  mockedAxios.post.mockResolvedValueOnce({
    data: {
      article: { ...defaultArticle, favorited: true },
    },
  });

  const result = await favoriteArticle(defaultArticle.slug);

  expect(mockedAxios.post.mock.calls.length).toBe(1);
  expect(result.slug).toMatch(defaultArticle.slug);
});

it('Should return article on unfavorite', async () => {
  mockedAxios.delete.mockResolvedValueOnce({
    data: {
      article: { ...defaultArticle, favorited: true },
    },
  });

  const result = await unfavoriteArticle(defaultArticle.slug);

  expect(mockedAxios.delete.mock.calls.length).toBe(1);
  expect(result.slug).toMatch(defaultArticle.slug);
});

it('Should get user', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      user: {
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      },
    },
  });

  const user = await getUser();
  expect(user).toHaveProperty('email', 'jake@jake.jake');
  expect(user).toHaveProperty('token', 'jwt.token.here');
});

it('Should get update settings errors', async () => {
  mockedAxios.put.mockRejectedValueOnce({ data: { errors: { x: ['y', 'z'] } } });

  const result = await updateSettings({ email: '', password: '', bio: '', image: null, username: '' });
  result.match({
    ok: () => fail(),
    err: (e) => {
      expect(e).toHaveProperty('x');
      expect(e['x']).toHaveLength(2);
    },
  });
});

it('Should get user on successful settings update', async () => {
  mockedAxios.put.mockResolvedValueOnce({
    data: {
      user: {
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      },
    },
  });

  const result = await updateSettings({ email: '', password: '', bio: '', image: null, username: '' });
  result.match({
    ok: (user) => {
      expect(user).toHaveProperty('email', 'jake@jake.jake');
      expect(user).toHaveProperty('token', 'jwt.token.here');
    },
    err: () => fail(),
  });
});

it('Should get signUp errors', async () => {
  mockedAxios.post.mockRejectedValueOnce({ response: { data: { errors: { x: ['y', 'z'] } } } });

  const result = await signUp({ email: '', password: '', username: '' });
  result.match({
    ok: () => fail(),
    err: (e) => {
      expect(e).toHaveProperty('x');
      expect(e['x']).toHaveLength(2);
    },
  });
});

it('Should get user on successful signup', async () => {
  mockedAxios.post.mockResolvedValueOnce({
    data: {
      user: {
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      },
    },
  });

  const result = await signUp({ email: '', password: '', username: '' });
  result.match({
    ok: (user) => {
      expect(user).toHaveProperty('email', 'jake@jake.jake');
      expect(user).toHaveProperty('token', 'jwt.token.here');
    },
    err: () => fail(),
  });
});

it('Should get errors on unsuccessful article creation', async () => {
  mockedAxios.post.mockRejectedValueOnce({ response: { data: { errors: { x: ['y', 'z'] } } } });

  const result = await createArticle({ title: '', body: '', description: '', tagList: [] });
  result.match({
    ok: () => fail(),
    err: (e) => {
      expect(e).toHaveProperty('x');
      expect(e['x']).toHaveLength(2);
    },
  });
});

it('Should get article on successful article creation', async () => {
  mockedAxios.post.mockResolvedValueOnce({
    data: {
      article: defaultArticle,
    },
  });

  const result = await createArticle({ title: '', body: '', description: '', tagList: [] });
  expect(result.isOk()).toBeTruthy();
  expect(result.unwrap().slug).toMatch(defaultArticle.slug);
});

it('Should get article', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      article: defaultArticle,
    },
  });

  const article = await getArticle('');
  expect(article.slug).toMatch(defaultArticle.slug);
});

it('Should get errors on unsuccessful article update', async () => {
  mockedAxios.put.mockRejectedValueOnce({ response: { data: { errors: { x: ['y', 'z'] } } } });

  const result = await updateArticle('', { title: '', body: '', description: '', tagList: [] });
  result.match({
    ok: () => fail(),
    err: (e) => {
      expect(e).toHaveProperty('x');
      expect(e['x']).toHaveLength(2);
    },
  });
});

it('Should get article on successful article creation', async () => {
  mockedAxios.put.mockResolvedValueOnce({
    data: {
      article: defaultArticle,
    },
  });

  const result = await updateArticle('', { title: '', body: '', description: '', tagList: [] });
  expect(result.isOk()).toBeTruthy();
  expect(result.unwrap().slug).toMatch(defaultArticle.slug);
});

it('Should get profile', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      profile: {
        username: 'the one',
        bio: 'and only',
        image: null,
        following: false,
      },
    },
  });

  const result = await getProfile('1');
  expect(result.username === 'the one').toBeTruthy();
});

it('Should get profile on follow', async () => {
  mockedAxios.post.mockResolvedValueOnce({
    data: {
      profile: {
        username: 'the one 2',
        bio: 'and only',
        image: null,
        following: false,
      },
    },
  });

  const result = await followUser('1');
  expect(result.username === 'the one 2').toBeTruthy();
});

it('Should get profile on unfollow', async () => {
  mockedAxios.delete.mockResolvedValueOnce({
    data: {
      profile: {
        username: 'the one 3',
        bio: 'and only',
        image: null,
        following: false,
      },
    },
  });

  const result = await unfollowUser('1');
  expect(result.username === 'the one 3').toBeTruthy();
});

it('Should get feed', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      articles: [defaultArticle, defaultArticle],
      articlesCount: 2,
    },
  });

  const result = await getFeed();
  expect(result.articles.length).toBe(2);
  expect(result.articlesCount).toBe(2);
});

it('Should get comments', async () => {
  mockedAxios.get.mockResolvedValueOnce({
    data: {
      comments: [defaultComment, defaultComment, defaultComment],
    },
  });
  const result = await getArticleComments('the slug');
  expect(result).toHaveLength(3);
  expect(mockedAxios.get.mock.calls).toHaveLength(1);
});

it('Should delete comment', async () => {
  mockedAxios.delete.mockResolvedValueOnce({});
  await deleteComment('the slug', 123);
  expect(mockedAxios.delete.mock.calls).toHaveLength(1);
});

it('Should add comment', async () => {
  mockedAxios.post.mockResolvedValueOnce({ data: { comment: defaultComment } });
  await createComment('the slug', 'The body');
  expect(mockedAxios.post.mock.calls).toHaveLength(1);
  expect(mockedAxios.post.mock.calls[0][1]).toHaveProperty('comment');
  expect(mockedAxios.post.mock.calls[0][1].comment).toHaveProperty('body', 'The body');
});

it('Should delete article', async () => {
  mockedAxios.delete.mockResolvedValueOnce({});
  await deleteArticle('the slug', 123);
  expect(mockedAxios.delete.mock.calls).toHaveLength(1);
});
