function Footer() {
  const year = new Date().getFullYear();

  return (
    <footer className="app-footer">
      <p>&copy; {year} Smart City</p>
    </footer>
  );
}

export default Footer;
