import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { ArticlePreview } from './ArticlePreview';

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
  tagList: ['tag1', 'tag2'],
  title: 'Test',
  updatedAt: new Date(),
};

it('Favorite button should be outlined if not favorited', () => {
  render(
    <MemoryRouter>
      <ArticlePreview article={defaultArticle} isSubmitting={false} />
    </MemoryRouter>
  );

  expect(screen.getByLabelText('Toggle Favorite').className.split(' ')).toContain('btn-outline-primary');
});

it('Favorite button should be primary if favorited', () => {
  render(
    <MemoryRouter>
      <ArticlePreview article={{ ...defaultArticle, favorited: true }} isSubmitting={false} />
    </MemoryRouter>
  );

  expect(screen.getByLabelText('Toggle Favorite').className.split(' ')).toContain('btn-primary');
});

it('Should display tags', () => {
  render(
    <MemoryRouter>
      <ArticlePreview article={defaultArticle} isSubmitting={false} />
    </MemoryRouter>
  );

  expect(screen.getByText('tag1')).toBeInTheDocument();
  expect(screen.getByText('tag2')).toBeInTheDocument();
});
