import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User, UserSettings } from '../../../types/user';
import { loadUser, logout } from '../../App/App.slice';
import * as R from 'ramda';
import { GenericErrors } from '../../../types/error';

export interface SettingsState {
  user: UserSettings;
  errors: GenericErrors;
  updating: boolean;
}

const initialState: SettingsState = {
  user: { username: '', email: '', password: null, bio: null, image: null },
  errors: {},
  updating: false,
};

const slice = createSlice({
  name: 'settings',
  initialState,
  reducers: {
    initializeSettings: () => initialState,
    updateField: (state, { payload: { name, value } }: PayloadAction<{ name: keyof UserSettings; value: string }>) => {
      state.user[name] = value;
    },
    updateErrors: (state, { payload: errors }: PayloadAction<GenericErrors>) => {
      state.errors = errors;
      state.updating = false;
    },
    startUpdate: (state) => {
      state.updating = true;
    },
  },
  extraReducers: {
    [loadUser.type]: (state, { payload: user }: PayloadAction<User>) => {
      state.user = { ...R.dissoc('token')(user), password: null };
      state.updating = false;
    },
    [logout.type]: () => initialState,
  },
});

export const { initializeSettings, updateField, updateErrors, startUpdate } = slice.actions;

export default slice.reducer;
