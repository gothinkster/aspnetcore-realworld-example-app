import { Err, Ok } from '@hqoss/monads';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { createArticle } from '../../../services/conduit';
import { store } from '../../../state/store';
import { initializeEditor } from '../../ArticleEditor/ArticleEditor.slice';
import { NewArticle } from './NewArticle';

jest.mock('../../../services/conduit.ts');

const mockedCreateArticle = createArticle as jest.Mock<ReturnType<typeof createArticle>>;

beforeEach(() => {
  act(() => {
    store.dispatch(initializeEditor());
    render(<NewArticle />);
  });
});

it('Should update errors if publish article fails', async () => {
  mockedCreateArticle.mockResolvedValueOnce(Err({ title: ['too smol', 'much fun'] }));
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(screen.getByText('title too smol')).toBeInTheDocument();
  expect(screen.getByText('title much fun')).toBeInTheDocument();
});

it('Should redirect to article if publish is successful', async () => {
  mockedCreateArticle.mockResolvedValueOnce(
    Ok({
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
      slug: 'test-ting',
      tagList: [],
      title: 'Test',
      updatedAt: new Date(),
    })
  );
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(location.hash).toMatch('#/article/test-ting');
});
