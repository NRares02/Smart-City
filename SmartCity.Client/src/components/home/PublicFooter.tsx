import smartCityLogo from '../../images/logo2_smartcity.png';

function PublicFooter() {
  const year = new Date().getFullYear();

  return (
    <footer className="public-footer">
      <div className="public-footer__top">
        <div className="public-footer__brand">
          <img src={smartCityLogo} alt="Smart City" />
          <span>Smart City</span>
        </div>

        <div className="public-footer__columns">
          <div className="public-footer__col">
            <h4>Platform</h4>
            <a href="#top">Home</a>
            <a href="#explore">Explore</a>
            <a href="#news">News</a>
          </div>
          <div className="public-footer__col">
            <h4>Resources</h4>
            <a href="#about">About</a>
            <a href="#top">Contact</a>
          </div>
          <div className="public-footer__col">
            <h4>Legal</h4>
            <a href="#top">Privacy</a>
            <a href="#top">Terms</a>
          </div>
        </div>
      </div>

      <div className="public-footer__bottom">&copy; {year} Smart City</div>
    </footer>
  );
}

export default PublicFooter;
