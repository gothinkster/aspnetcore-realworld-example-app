import { Err, Ok } from '@hqoss/monads';
import { act, fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import { MemoryRouter, Route } from 'react-router-dom';
import { getArticle, updateArticle } from '../../../services/conduit';
import { store } from '../../../state/store';
import { loadUser } from '../../App/App.slice';
import { initializeEditor } from '../../ArticleEditor/ArticleEditor.slice';
import { EditArticle } from './EditArticle';

jest.mock('../../../services/conduit.ts');

const mockedGetArticle = getArticle as jest.Mock<ReturnType<typeof getArticle>>;
const mockedUpdateArticle = updateArticle as jest.Mock<ReturnType<typeof updateArticle>>;

const defaultArticle = {
  author: {
    bio: null,
    following: false,
    image: null,
    username: 'jake',
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

const defaultUser = {
  email: 'jake@jake.jake',
  token: 'jwt.token.here',
  username: 'jake',
  bio: 'I work at statefarm',
  image: null,
};

beforeEach(async () => {
  await act(async () => {
    store.dispatch(loadUser(defaultUser));
    store.dispatch(initializeEditor());
  });
});

async function renderWithPath(articleSlug: string) {
  await act(async () => {
    render(
      <MemoryRouter initialEntries={[`/${articleSlug}`]}>
        <Route path='/:slug'>
          <EditArticle />
        </Route>
      </MemoryRouter>
    );
  });
}

it('Should redirect to home if article load fails', async () => {
  location.hash = '#/editor/1234';
  mockedGetArticle.mockRejectedValueOnce({});

  await renderWithPath('1234');

  expect(location.hash).toMatch(/#\/$/);
});

it('Should redirect to home if article owner is not the logged user', async () => {
  location.hash = '#/editor/1234';
  mockedGetArticle.mockResolvedValueOnce(defaultArticle);
  store.dispatch(loadUser({ ...defaultUser, username: '3434' }));
  await renderWithPath('1234');

  expect(location.hash).toMatch(/#\/$/);
});

it('Should load article', async () => {
  location.hash = '#/editor/1234';
  mockedGetArticle.mockResolvedValueOnce({ ...defaultArticle, tagList: ['123', '456'] });
  await renderWithPath('1234');

  expect(store.getState().editor.loading).toBeFalsy();
  expect(store.getState().editor.article.tagList).toHaveLength(2);
  expect(store.getState().editor.article.title).toMatch(defaultArticle.title);
});

it('Should update errors if publish article fails', async () => {
  mockedGetArticle.mockResolvedValueOnce(defaultArticle);
  mockedUpdateArticle.mockResolvedValueOnce(Err({ title: ['too smol', 'much fun'] }));

  await renderWithPath('1234');
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(screen.getByText('title too smol')).toBeInTheDocument();
  expect(screen.getByText('title much fun')).toBeInTheDocument();
});

it('Should redirect to article if update is successful', async () => {
  mockedGetArticle.mockResolvedValueOnce(defaultArticle);
  mockedUpdateArticle.mockResolvedValueOnce(Ok(defaultArticle));

  await renderWithPath('1234');
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(location.hash).toMatch(`#/article/${defaultArticle.slug}`);
});
