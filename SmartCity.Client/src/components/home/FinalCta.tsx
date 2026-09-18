import { useNavigate } from 'react-router-dom';
import { isAuthenticated } from '../../utils/storage';

function FinalCta() {
  const navigate = useNavigate();
  const authed = isAuthenticated();

  return (
    <section className="final-cta">
      <h2>Ready to connect with your city?</h2>
      <p>Join Smart City today and stay in tune with everything happening around you.</p>
      <div className="final-cta__actions">
        {authed ? (
          <button type="button" className="final-cta__primary" onClick={() => navigate('/dashboard')}>
            Go to Dashboard
          </button>
        ) : (
          <>
            <button type="button" className="final-cta__primary" onClick={() => navigate('/auth')}>
              Create Account
            </button>
            <button type="button" className="final-cta__ghost" onClick={() => navigate('/auth')}>
              Sign In
            </button>
          </>
        )}
      </div>
    </section>
  );
}

export default FinalCta;
