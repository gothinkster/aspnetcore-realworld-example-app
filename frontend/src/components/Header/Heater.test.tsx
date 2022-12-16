import { act, render, screen } from '@testing-library/react';
import { store } from '../../state/store';
import { initializeApp, loadUser } from '../App/App.slice';
import { Header } from './Header';

it('Should render', () => {
  render(<Header />);
});

describe('Header for guest', () => {
  beforeAll(() => {
    act(() => {
      store.dispatch(initializeApp);
    });
  });

  beforeEach(() => {
    render(<Header />);
  });

  it('Should render Sign in link', () => {
    expectLinkByText('Sign in', 'login');
  });

  it('Should render Sign up link', () => {
    expectLinkByText('Sign up', 'register');
  });

  it('Should not render New Article link', () => {
    expectEmptyQueryByText('New Article');
  });

  it('Should not render Settings link', () => {
    expectEmptyQueryByText('Settings');
  });
});

describe('Header for user', () => {
  beforeAll(() => {
    act(() => {
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

  beforeEach(() => render(<Header />));

  it('Should render New Article link', () => {
    expectLinkByText('New Article', 'editor');
  });

  it('Should render Settings link', () => {
    expectLinkByText('Settings', 'settings');
  });

  it('Should render user link', () => {
    expectLinkByText('jake', 'profile/jake');
  });

  it('Should not render Sign in link', () => {
    expectEmptyQueryByText('Sign in');
  });

  it('Should not render Sign up link', () => {
    expectEmptyQueryByText('Sign up');
  });
});

function expectLinkByText(text: string, href: string) {
  const link = screen.getByText(text);
  expect(link).toBeInTheDocument();
  expect(link.nodeName).toMatch('A');
  expect(link.getAttribute('href')).toMatch('#/' + href);
}

function expectEmptyQueryByText(text: string) {
  expect(screen.queryAllByText(text)).toHaveLength(0);
}
