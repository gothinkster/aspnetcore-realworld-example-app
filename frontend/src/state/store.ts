import { Action, configureStore } from '@reduxjs/toolkit';
import app from '../components/App/App.slice';
import home from '../components/Pages/Home/Home.slice';
import login from '../components/Pages/Login/Login.slice';
import settings from '../components/Pages/Settings/Settings.slice';
import register from '../components/Pages/Register/Register.slice';
import editor from '../components/ArticleEditor/ArticleEditor.slice';
import articleViewer from '../components/ArticlesViewer/ArticlesViewer.slice';
import profile from '../components/Pages/ProfilePage/ProfilePage.slice';
import articlePage from '../components/Pages/ArticlePage/ArticlePage.slice';

const middlewareConfiguration = { serializableCheck: false };

export const store = configureStore({
  reducer: { app, home, login, settings, register, editor, articleViewer, profile, articlePage },
  devTools: {
    name: 'Conduit',
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware(middlewareConfiguration),
});
export type State = ReturnType<typeof store.getState>;

export function dispatchOnCall(action: Action) {
  return () => store.dispatch(action);
}
