import { act, fireEvent, render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { favoriteArticle, getArticles, getFeed, getTags, unfavoriteArticle } from '../../../services/conduit';
import { store } from '../../../state/store';
import { initializeApp, loadUser } from '../../App/App.slice';
import { Home } from './Home';
import { changeTab } from './Home.slice';

jest.mock('../../../services/conduit');

const mockedGetArticles = getArticles as jest.Mock<ReturnType<typeof getArticles>>;
const mockedGetTags = getTags as jest.Mock<ReturnType<typeof getTags>>;
const mockedFavoriteArticle = favoriteArticle as jest.Mock<ReturnType<typeof favoriteArticle>>;
const mockedUnfavoriteArticle = unfavoriteArticle as jest.Mock<ReturnType<typeof unfavoriteArticle>>;
const mockedGetFeed = getFeed as jest.Mock<ReturnType<typeof getFeed>>;

const defaultArticle = {
  author: {
    bio: null,
    following: false,
    image: 'https://static.productionready.io/images/smiley-cyrus.jpg',
    username: 'Jazmin Martinez',
  },
  body: 'Test 1',
  createdAt: new Date(),
  description: 'Test 1',
  favorited: false,
  favoritesCount: 0,
  slug: 'test-pmy91z',
  tagList: [],
  title: 'Test',
  updatedAt: new Date(),
};

beforeEach(async () => {
  await act(async () => {
    store.dispatch(initializeApp());
    store.dispatch(changeTab('Global Feed'));
  });
});

it('Should load articles', async () => {
  mockedGetArticles.mockResolvedValueOnce({
    articles: [
      defaultArticle,
      {
        ...defaultArticle,
        description: 'Test 2',
        slug: 'test-2344',
        author: { ...defaultArticle.author, image: null },
      },
    ],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: ['twitter', 'facebook', 'google'] });

  await act(async () => {
    await render(
      <MemoryRouter>
        <MemoryRouter>
          <Home />
        </MemoryRouter>
      </MemoryRouter>
    );
  });

  screen.getByText('google');
  screen.getByText('Test 1');
  screen.getByText('Test 2');
  expect(store.getState().home.selectedTab).toMatch('Global Feed');
});

it('Should show message if there are no articles', async () => {
  mockedGetArticles.mockResolvedValueOnce({
    articles: [],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });

  await act(async () => {
    await render(
      <MemoryRouter>
        <MemoryRouter>
          <Home />
        </MemoryRouter>
      </MemoryRouter>
    );
  });
  expect(screen.getByText('No articles are here... yet.')).toBeInTheDocument();
});

it('Should load feed articles if user is logged in', async () => {
  mockedGetFeed.mockResolvedValueOnce({
    articles: [
      defaultArticle,
      {
        ...defaultArticle,
        description: 'Test 2',
        slug: 'test-2344',
        author: { ...defaultArticle.author, image: null },
      },
    ],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });

  await act(async () => {
    store.dispatch(
      loadUser({
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      })
    );
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  expect(store.getState().home.selectedTab).toMatch('Your Feed');
});

it('Should redirect to login on favorite if the user is not logged in', async () => {
  mockedGetArticles.mockResolvedValueOnce({
    articles: [defaultArticle],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  mockedFavoriteArticle.mockResolvedValueOnce({ ...defaultArticle, favorited: true });

  await act(async () => {
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
    expect(store.getState().articleViewer.articles.unwrap()[0].isSubmitting).toBe(false);
  });

  expect(mockedFavoriteArticle.mock.calls.length).toBe(0);
  mockedFavoriteArticle.mockClear();
  expect(location.hash).toMatch('#/login');
});

it('Should favorite article', async () => {
  mockedGetFeed.mockResolvedValueOnce({
    articles: [defaultArticle],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  mockedFavoriteArticle.mockResolvedValueOnce({ ...defaultArticle, favorited: true });

  await act(async () => {
    store.dispatch(
      loadUser({
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      })
    );
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
    expect(store.getState().articleViewer.articles.unwrap()[0].isSubmitting).toBe(true);
  });

  expect(store.getState().articleViewer.articles.unwrap()[0].isSubmitting).toBe(false);
  expect(store.getState().articleViewer.articles.unwrap()[0].article.favorited).toBe(true);
});

it('Should unfavorite article', async () => {
  mockedGetFeed.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, favorited: true }],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  mockedUnfavoriteArticle.mockResolvedValueOnce({ ...defaultArticle, favorited: false });

  await act(async () => {
    store.dispatch(
      loadUser({
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      })
    );
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
    expect(store.getState().articleViewer.articles.unwrap()[0].isSubmitting).toBe(true);
  });

  expect(store.getState().articleViewer.articles.unwrap()[0].isSubmitting).toBe(false);
  expect(store.getState().articleViewer.articles.unwrap()[0].article.favorited).toBe(false);
});

it('Should load another page', async () => {
  mockedGetArticles.mockResolvedValueOnce({
    articles: [defaultArticle],
    articlesCount: 100,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });

  await act(async () => {
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  mockedGetArticles.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, title: 'After change' }],
    articlesCount: 100,
  });

  await act(async () => {
    fireEvent.click(screen.getByLabelText(/Go to page number 5/));
  });

  expect(store.getState().articleViewer.currentPage).toBe(5);
  expect(screen.getByText('After change')).toBeInTheDocument();
  expect(mockedGetArticles.mock.calls[1][0]).toHaveProperty('offset', 40);
});

it('Should change tabs', async () => {
  mockedGetFeed.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, favorited: true }],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: ['the real tag'] });
  mockedUnfavoriteArticle.mockResolvedValueOnce({ ...defaultArticle, favorited: false });

  await act(async () => {
    store.dispatch(
      loadUser({
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      })
    );
    await render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    );
  });

  mockedGetArticles.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, favorited: true }],
    articlesCount: 0,
  });

  await act(async () => {
    fireEvent.click(screen.getByText('Global Feed'));
  });

  expect(store.getState().home.selectedTab).toMatch('Global Feed');

  mockedGetFeed.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, favorited: true }],
    articlesCount: 0,
  });

  await act(async () => {
    fireEvent.click(screen.getByText('Your Feed'));
  });

  expect(store.getState().home.selectedTab).toMatch('Your Feed');

  mockedGetArticles.mockResolvedValueOnce({
    articles: [{ ...defaultArticle, favorited: true }],
    articlesCount: 0,
  });

  await act(async () => {
    fireEvent.click(screen.getByText('the real tag'));
  });

  expect(screen.getByText('# the real tag')).toHaveClass('active');
  expect(mockedGetArticles.mock.calls).toHaveLength(2);
  expect(mockedGetArticles.mock.calls[1][0]).toHaveProperty('tag', 'the real tag');
});
