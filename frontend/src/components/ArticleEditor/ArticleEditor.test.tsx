import { act, fireEvent, render, screen } from '@testing-library/react';
import { store } from '../../state/store';
import { ArticleEditor } from './ArticleEditor';
import { addTag, initializeEditor, updateField } from './ArticleEditor.slice';

beforeEach(() => {
  act(() => {
    store.dispatch(initializeEditor());
    render(<ArticleEditor onSubmit={(ev) => ev.preventDefault()} />);
  });
});

it('Should update article text fields', async () => {
  await act(async () => {
    fireEvent.change(screen.getByPlaceholderText('Article Title'), { target: { value: 'testTitle' } });
    fireEvent.change(screen.getByPlaceholderText("What's this article about?"), {
      target: { value: 'testDescription' },
    });
    fireEvent.change(screen.getByPlaceholderText('Write your article (in markdown)'), {
      target: { value: 'testBody' },
    });
    store.dispatch(updateField({ name: 'tagList', value: 'df' }));
  });

  expect(store.getState().editor.article.title).toMatch('testTitle');
  expect(store.getState().editor.article.description).toMatch('testDescription');
  expect(store.getState().editor.article.body).toMatch('testBody');
});

it('Should update article tag list field', async () => {
  await act(async () => {
    const enterTagsElement = screen.getByPlaceholderText('Enter Tags');
    fireEvent.keyDown(enterTagsElement, { key: 'Enter' });
    fireEvent.keyDown(enterTagsElement, { key: 'A' });

    fireEvent.change(enterTagsElement, { target: { value: 'tag1' } });
    fireEvent.keyUp(enterTagsElement, { key: 'Enter' });

    fireEvent.change(enterTagsElement, { target: { value: 'tag2' } });
    fireEvent.keyUp(enterTagsElement, { key: 'Enter' });

    fireEvent.change(enterTagsElement, { target: { value: 'tag3' } });
    fireEvent.keyUp(enterTagsElement, { key: 'Enter' });

    store.dispatch(addTag());

    fireEvent.keyUp(enterTagsElement, { key: 'k' });
  });

  expect(store.getState().editor.article.tagList).toHaveLength(3);
  expect(store.getState().editor.article.tagList).toContain('tag1');
  expect(store.getState().editor.article.tagList).toContain('tag2');
  expect(store.getState().editor.article.tagList).toContain('tag3');

  await act(async () => {
    fireEvent.click(screen.getByText('tag2'));
  });

  expect(store.getState().editor.article.tagList).toHaveLength(2);
  expect(store.getState().editor.article.tagList).toContain('tag1');
  expect(store.getState().editor.article.tagList).toContain('tag3');
});
