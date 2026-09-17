import { Link } from 'react-router-dom';

function NotFoundPage() {
  return (
    <section className="page page--not-found">
      <h1>404</h1>
      <p>The page you are looking for does not exist.</p>
      <Link to="/">Back to home</Link>
    </section>
  );
}

export default NotFoundPage;
