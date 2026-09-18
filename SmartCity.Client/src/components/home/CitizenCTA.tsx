import { useNavigate } from 'react-router-dom';
import { isAuthenticated } from '../../utils/storage';
import { ReportsIcon } from '../dashboard/icons';

function CitizenCTA() {
  const navigate = useNavigate();
  const authed = isAuthenticated();

  function handleReport() {
    navigate(authed ? '/reports/new' : '/auth');
  }

  return (
    <section className="citizen-cta">
      <div className="citizen-cta__icon">
        <ReportsIcon size={26} />
      </div>
      <h2>See something that needs attention?</h2>
      <p>Report a local issue and help make your city better.</p>
      <button type="button" className="citizen-cta__button" onClick={handleReport}>
        Report a Problem
      </button>
    </section>
  );
}

export default CitizenCTA;
