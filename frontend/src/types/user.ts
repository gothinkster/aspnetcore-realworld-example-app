import axios from 'axios';
import { Decoder, nullable, object, string } from 'decoders';
import { loadUser } from '../components/App/App.slice';
import { store } from '../state/store';

export interface PublicUser {
  username: string;
  bio: string | null;
  image: string | null;
}

export interface User extends PublicUser {
  email: string;
  token: string;
}

export const userDecoder: Decoder<User> = object({
  email: string,
  token: string,
  username: string,
  bio: nullable(string),
  image: nullable(string),
});

export interface UserSettings extends PublicUser {
  email: string;
  password: string | null;
}

export interface UserForRegistration {
  username: string;
  email: string;
  password: string;
}

export function loadUserIntoApp(user: User) {
  localStorage.setItem('token', user.token);
  axios.defaults.headers.Authorization = `Token ${user.token}`;
  store.dispatch(loadUser(user));
}
