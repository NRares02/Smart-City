import { NavLink } from 'react-router-dom';

function Header() {
  return (
    <header className="app-header">
      <div className="app-header__brand">Smart City</div>
      <nav className="app-header__nav">
        <NavLink to="/" end>
          Home
        </NavLink>
      </nav>
    </header>
  );
}

export default Header;
