import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { GenericErrors } from '../../../types/error';
import { UserForRegistration } from '../../../types/user';

export interface RegisterState {
  user: UserForRegistration;
  errors: GenericErrors;
  signingUp: boolean;
}

const initialState: RegisterState = {
  user: {
    username: '',
    email: '',
    password: '',
  },
  errors: {},
  signingUp: false,
};

const slice = createSlice({
  name: 'register',
  initialState,
  reducers: {
    initializeRegister: () => initialState,
    updateField: (
      state,
      { payload: { name, value } }: PayloadAction<{ name: keyof RegisterState['user']; value: string }>
    ) => {
      state.user[name] = value;
    },
    updateErrors: (state, { payload: errors }: PayloadAction<GenericErrors>) => {
      state.errors = errors;
      state.signingUp = false;
    },
    startSigningUp: (state) => {
      state.signingUp = true;
    },
  },
});

export const { initializeRegister, updateField, updateErrors, startSigningUp } = slice.actions;

export default slice.reducer;
