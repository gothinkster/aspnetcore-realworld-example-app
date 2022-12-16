import { None, Option, Some } from '@hqoss/monads';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Profile } from '../../../types/profile';

export interface ProfilePageState {
  profile: Option<Profile>;
  submitting: boolean;
}

const initialState: ProfilePageState = {
  profile: None,
  submitting: false,
};

const slice = createSlice({
  name: 'profile',
  initialState,
  reducers: {
    initializeProfile: () => initialState,
    loadProfile: (state, { payload: profile }: PayloadAction<Profile>) => {
      state.profile = Some(profile);
      state.submitting = false;
    },
    startSubmitting: (state) => ({ ...state, submitting: true }),
  },
});

export const { initializeProfile, loadProfile, startSubmitting } = slice.actions;

export default slice.reducer;
