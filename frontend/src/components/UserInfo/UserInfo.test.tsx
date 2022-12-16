import { act, fireEvent, render, screen } from '@testing-library/react';
import { store } from '../../state/store';
import { loadUser } from '../App/App.slice';
import { UserInfo } from './UserInfo';

beforeEach(async () => {
  await act(async () => {
    store.dispatch(
      loadUser({
        email: 'jake@jake.jake',
        token: 'jwt.token.here',
        username: 'jake',
        bio: 'I work at statefarm',
        image: null,
      })
    );
  });
});

it('Should toggle favorite', async () => {
  const mockOnFollowToggle = jest.fn();
  await act(async () => {
    render(
      <UserInfo
        user={{
          username: 'test jack',
          bio: 'I work at statefarm',
          image: null,
          following: false,
        }}
        onFollowToggle={mockOnFollowToggle}
      />
    );

    fireEvent.click(screen.getByText('Follow test jack'));
  });

  expect(mockOnFollowToggle.mock.calls).toHaveLength(1);
});

it('Should toggle favorite for followed', async () => {
  const mockOnFollowToggle = jest.fn();
  await act(async () => {
    render(
      <UserInfo
        user={{
          username: 'test jack',
          bio: 'I work at statefarm',
          image: null,
          following: true,
        }}
        onFollowToggle={mockOnFollowToggle}
      />
    );

    fireEvent.click(screen.getByText('Unfollow test jack'));
  });

  expect(mockOnFollowToggle.mock.calls).toHaveLength(1);
});

it('Should trigger edit settings', async () => {
  const mockOnEditSettings = jest.fn();
  await act(async () => {
    render(
      <UserInfo
        user={{
          username: 'jake',
          bio: 'I work at statefarm',
          image: null,
          following: true,
        }}
        onEditSettings={mockOnEditSettings}
      />
    );

    fireEvent.click(screen.getByText('Edit Profile Settings'));
  });

  expect(mockOnEditSettings.mock.calls).toHaveLength(1);
});
