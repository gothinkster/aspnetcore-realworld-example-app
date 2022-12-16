import { store } from '../../../state/store';
import { Settings } from './Settings';
import { initializeSettings, startUpdate, updateErrors } from './Settings.slice';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { updateSettings } from '../../../services/conduit';
import { Err, Ok } from '@hqoss/monads';
import axios from 'axios';
import { loadUser } from '../../App/App.slice';

jest.mock('../../../services/conduit.ts');
jest.mock('axios');

const mockedUpdateSettings = updateSettings as jest.Mock<ReturnType<typeof updateSettings>>;

beforeEach(() => {
  act(() => {
    store.dispatch(initializeSettings());
    render(<Settings />);
  });
});

it('Should update user fields', async () => {
  await act(async () => {
    fireEvent.change(screen.getByPlaceholderText('URL of profile picture'), { target: { value: 'testImage' } });
    fireEvent.change(screen.getByPlaceholderText('Your Name'), { target: { value: 'testUsername' } });
    fireEvent.change(screen.getByPlaceholderText('Short bio about you'), { target: { value: 'testBio' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'testEmail' } });
    fireEvent.change(screen.getByPlaceholderText('Password'), { target: { value: 'testPassword' } });
  });

  expect(store.getState().settings.user.image).toMatch('testImage');
  expect(store.getState().settings.user.username).toMatch('testUsername');
  expect(store.getState().settings.user.bio).toMatch('testBio');
  expect(store.getState().settings.user.email).toMatch('testEmail');
  expect(store.getState().settings.user.password).toMatch('testPassword');
});

it('Should show errors', async () => {
  await act(async () => {
    store.dispatch(updateErrors({ 'email or password': ['is invalid', 'is empty'] }));
  });

  expect(screen.getByText('email or password is invalid')).toBeInTheDocument();
  expect(screen.getByText('email or password is empty')).toBeInTheDocument();
});

it('Should disable fields during update and enabled afterwards with errors', async () => {
  mockedUpdateSettings.mockResolvedValueOnce(Err({ 'email or password': ['is invalid2', 'is empty3'] }));

  await act(async () => {
    store.dispatch(startUpdate());
  });

  expect(screen.getByPlaceholderText('URL of profile picture')).toBeDisabled();
  expect(screen.getByPlaceholderText('Your Name')).toBeDisabled();
  expect(screen.getByPlaceholderText('Short bio about you')).toBeDisabled();
  expect(screen.getByPlaceholderText('Email')).toBeDisabled();
  expect(screen.getByPlaceholderText('Password')).toBeDisabled();

  await act(async () => {
    store.dispatch(initializeSettings());
  });

  await act(async () => {
    fireEvent.click(screen.getByText('Update Settings'));
  });

  expect(screen.getByPlaceholderText('URL of profile picture')).not.toBeDisabled();
  expect(screen.getByPlaceholderText('Your Name')).not.toBeDisabled();
  expect(screen.getByPlaceholderText('Short bio about you')).not.toBeDisabled();
  expect(screen.getByPlaceholderText('Email')).not.toBeDisabled();
  expect(screen.getByPlaceholderText('Password')).not.toBeDisabled();

  expect(screen.getByText('email or password is invalid2')).toBeInTheDocument();
  expect(screen.getByText('email or password is empty3')).toBeInTheDocument();
});

it('Should redirect to home if settings update succeeds', async () => {
  mockedUpdateSettings.mockResolvedValueOnce(
    Ok({
      email: 'jake@jake.jakesettings',
      token: 'jwt.token.here',
      username: 'jake',
      bio: 'I work at statefarm',
      image: null,
    })
  );

  await act(async () => {
    fireEvent.click(screen.getByText('Update Settings'));
  });

  expect(location.hash).toMatch('#/');
  expect(store.getState().app.user.unwrap().email).toMatch('jake@jake.jakesettings');
});

it('Should logout', async () => {
  const user = {
    email: 'jake@jake.jake',
    token: 'jwt.token.here',
    username: 'jake',
    bio: 'I work at statefarm',
    image: null,
  };
  localStorage.setItem('token', user.token);
  axios.defaults.headers.Authorization = `Token ${user.token}`;

  await act(async () => {
    store.dispatch(loadUser(user));
    fireEvent.click(screen.getByText('Or click here to logout.'));
  });

  expect(location.hash).toMatch('#/');
  expect(localStorage.getItem('token')).toBeNull();
  expect(axios.defaults.headers).not.toHaveProperty('Authorization');
  expect(store.getState().app.user.isSome()).toBe(false);
});
