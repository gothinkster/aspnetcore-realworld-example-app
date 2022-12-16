import { Decoder, string, nullable, boolean, object, hardcoded, either } from 'decoders';
import { PublicUser } from './user';

export interface Profile extends PublicUser {
  following: boolean;
}

export const profileDecoder: Decoder<Profile> = object({
  username: string,
  bio: nullable(string),
  image: nullable(string),
  following: either(boolean, hardcoded(false)),
});
