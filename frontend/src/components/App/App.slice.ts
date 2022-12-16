import { None, Option, Some } from '@hqoss/monads';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../../types/user';

export interface AppState {
  user: Option<User>;
  loading: boolean;
}

const initialState: AppState = {
  user: None,
  loading: true,
};

const slice = createSlice({
  name: 'app',
  initialState,
  reducers: {
    initializeApp: () => initialState,
    loadUser: (state, { payload: user }: PayloadAction<User>) => {
      state.user = Some(user);
      state.loading = false;
    },
    logout: (state) => {
      state.user = None;
    },
    endLoad: (state) => {
      state.loading = false;
    },
  },
});

export const { loadUser, logout, endLoad, initializeApp } = slice.actions;

export default slice.reducer;
