import { Fragment } from 'react';
import { HashRouter, NavLink } from 'react-router-dom';
import { useStore } from '../../state/storeHooks';
import { User } from '../../types/user';

export function Header() {
  const { user } = useStore(({ app }) => app);

  return (
    <nav className='navbar navbar-light'>
      <div className='container'>
        <a className='navbar-brand' href='/#/'>
          conduit
        </a>
        <ul className='nav navbar-nav pull-xs-right'>
          <HashRouter>
            <NavItem text='Home' href='/' />

            {user.match({
              none: () => <GuestLinks />,
              some: (user) => <UserLinks user={user} />,
            })}
          </HashRouter>
        </ul>
      </div>
    </nav>
  );
}

function NavItem({ text, href, icon }: { text: string; href: string; icon?: string }) {
  return (
    <li className='nav-item'>
      <NavLink exact to={href} activeClassName='active' className='nav-link'>
        {icon && <i className={icon}></i>}&nbsp;
        {text}
      </NavLink>
    </li>
  );
}

function GuestLinks() {
  return (
    <Fragment>
      <NavItem text='Sign in' href='/login' />
      <NavItem text='Sign up' href='/register' />
    </Fragment>
  );
}

function UserLinks({ user: { username } }: { user: User }) {
  return (
    <Fragment>
      <NavItem text='New Article' href='/editor' icon='ion-compose' />
      <NavItem text='Settings' href='/settings' icon='ion-gear-a' />
      <NavItem text={`${username}`} href={`/profile/${username}`} />
    </Fragment>
  );
}
