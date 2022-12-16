import { None, Option, Some } from '@hqoss/monads';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Article, MultipleArticles } from '../../types/article';
import * as R from 'ramda';

export interface ArticleViewerArticle {
  article: Article;
  isSubmitting: boolean;
}

export interface ArticleViewerState {
  articles: Option<readonly ArticleViewerArticle[]>;
  currentPage: number;
  articlesCount: number;
}

const initialState: ArticleViewerState = {
  articles: None,
  currentPage: 1,
  articlesCount: 0,
};

const slice = createSlice({
  name: 'articleViewer',
  initialState,
  reducers: {
    startLoadingArticles: () => initialState,
    loadArticles: (state, { payload: { articles, articlesCount } }: PayloadAction<MultipleArticles>) => {
      state.articles = Some(articles.map((article) => ({ article, isSubmitting: false })));
      state.articlesCount = articlesCount;
    },
    startSubmittingFavorite: (state, { payload: index }: PayloadAction<number>) => {
      state.articles = state.articles.map(R.adjust(index, R.assoc('isSubmitting', true)));
    },
    endSubmittingFavorite: (
      state,
      { payload: { article, index } }: PayloadAction<{ index: number; article: Article }>
    ) => {
      state.articles = state.articles.map(R.update<ArticleViewerArticle>(index, { article, isSubmitting: false }));
    },
    changePage: (state, { payload: page }: PayloadAction<number>) => {
      state.currentPage = page;
      state.articles = None;
    },
  },
});

export const { startLoadingArticles, loadArticles, startSubmittingFavorite, endSubmittingFavorite, changePage } =
  slice.actions;

export default slice.reducer;
