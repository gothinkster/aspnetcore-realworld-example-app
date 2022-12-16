import { Decoder, iso8601, number, object, string } from 'decoders';
import { Profile, profileDecoder } from './profile';

export interface Comment {
  id: number;
  createdAt: Date;
  updatedAt: Date;
  body: string;
  author: Profile;
}

export const commentDecoder: Decoder<Comment> = object({
  id: number,
  createdAt: iso8601,
  updatedAt: iso8601,
  body: string,
  author: profileDecoder,
});
