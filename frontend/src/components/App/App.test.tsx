import { act, render, screen } from '@testing-library/react';
import axios from 'axios';
import { getArticles, getFeed, getTags, getUser } from '../../services/conduit';
import { store } from '../../state/store';
import { App } from './App';
import { initializeApp } from './App.slice';

jest.mock('../../services/conduit');
jest.mock('axios');

const mockedGetArticles = getArticles as jest.Mock<ReturnType<typeof getArticles>>;
const mockedGetFeed = getFeed as jest.Mock<ReturnType<typeof getFeed>>;
const mockedGetTags = getTags as jest.Mock<ReturnType<typeof getTags>>;
const mockedGetUser = getUser as jest.Mock<ReturnType<typeof getUser>>;

it('Should render home', async () => {
  act(() => {
    store.dispatch(initializeApp());
  });
  mockedGetArticles.mockResolvedValueOnce({
    articles: [],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  mockedGetUser.mockImplementationOnce(jest.fn());
  localStorage.clear();

  await act(async () => {
    await render(<App />);
  });

  expect(screen.getByText('A place to share your knowledge.')).toBeInTheDocument();
  expect(mockedGetUser.mock.calls.length).toBe(0);
  mockedGetUser.mockClear();
});

it('Should get user if token is on storage', async () => {
  act(() => {
    store.dispatch(initializeApp());
  });
  mockedGetUser.mockResolvedValueOnce({
    email: 'jake@jake.jake',
    token: 'my-token',
    username: 'jake',
    bio: 'I work at statefarm',
    image: null,
  });
  mockedGetFeed.mockResolvedValueOnce({
    articles: [],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  localStorage.setItem('token', 'my-token');

  await act(async () => {
    await render(<App />);
  });

  expect(axios.defaults.headers.Authorization).toMatch('Token my-token');
  const optionUser = store.getState().app.user;
  expect(optionUser.isSome()).toBe(true);

  const user = optionUser.unwrap();
  expect(user).toHaveProperty('email', 'jake@jake.jake');
  expect(user).toHaveProperty('token', 'my-token');
  expect(store.getState().app.loading).toBe(false);
});

it('Should end load if get user fails', async () => {
  await act(async () => {
    store.dispatch(initializeApp());
  });
  mockedGetUser.mockRejectedValueOnce({});
  mockedGetArticles.mockResolvedValueOnce({
    articles: [],
    articlesCount: 0,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  localStorage.setItem('token', 'my-token');

  await act(async () => {
    await render(<App />);
  });

  expect(store.getState().app.user.isNone()).toBeTruthy();
  expect(store.getState().app.loading).toBe(false);
});
