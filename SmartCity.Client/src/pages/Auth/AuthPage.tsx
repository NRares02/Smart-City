import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SignInForm from '../../components/auth/SignInForm';
import SignUpForm from '../../components/auth/SignUpForm';
import smartCityLogo from '../../images/logo2_smartcity.png';
import './AuthPage.css';

type AuthTab = 'signin' | 'signup';

function AuthPage() {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<AuthTab>('signin');
  const [signedUpEmail, setSignedUpEmail] = useState<string | undefined>(undefined);

  function handleSignUpSuccess(email: string) {
    setSignedUpEmail(email);
    setActiveTab('signin');
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <img className="auth-card__logo" src={smartCityLogo} alt="Smart City" />

        <div className="auth-tabs">
          <button
            type="button"
            className={activeTab === 'signin' ? 'auth-tabs__btn is-active' : 'auth-tabs__btn'}
            onClick={() => setActiveTab('signin')}
          >
            Sign In
          </button>
          <button
            type="button"
            className={activeTab === 'signup' ? 'auth-tabs__btn is-active' : 'auth-tabs__btn'}
            onClick={() => setActiveTab('signup')}
          >
            Sign Up
          </button>
        </div>

        {activeTab === 'signin' ? (
          <SignInForm initialEmail={signedUpEmail} onSuccess={() => navigate('/dashboard')} />
        ) : (
          <SignUpForm onRegistered={handleSignUpSuccess} />
        )}
      </div>
    </div>
  );
}

export default AuthPage;
