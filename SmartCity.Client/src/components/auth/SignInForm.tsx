import { useState, type FormEvent } from 'react';
import { getAuthErrorMessage, login, persistSession } from '../../services/authService';
import { GoogleIcon, MicrosoftIcon } from './SocialIcons';
import EyeIcon from './EyeIcon';

interface SignInFormProps {
  initialEmail?: string;
  onSuccess: () => void;
}

interface FormErrors {
  email?: string;
  password?: string;
  form?: string;
}

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function SignInForm({ initialEmail, onSuccess }: SignInFormProps) {
  const [email, setEmail] = useState(initialEmail ?? '');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const [errors, setErrors] = useState<FormErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  function validate(): boolean {
    const nextErrors: FormErrors = {};
    if (!email.trim()) {
      nextErrors.email = 'Email is required.';
    } else if (!EMAIL_PATTERN.test(email)) {
      nextErrors.email = 'Enter a valid email address.';
    }
    if (!password) {
      nextErrors.password = 'Password is required.';
    }
    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!validate()) {
      return;
    }

    setIsSubmitting(true);
    setErrors({});
    try {
      const response = await login({ email: email.trim(), password });
      persistSession(response);
      onSuccess();
    } catch (error) {
      setErrors({ form: getAuthErrorMessage(error, 'Unable to sign in. Please try again.') });
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="auth-form" onSubmit={handleSubmit} noValidate>
      {initialEmail && !errors.form && (
        <p className="auth-form__notice">Account created. Please sign in.</p>
      )}
      {errors.form && <p className="auth-form__error auth-form__error--general">{errors.form}</p>}

      <label className="auth-field">
        <span>Email</span>
        <input
          type="email"
          value={email}
          onChange={(event) => setEmail(event.target.value)}
          placeholder="you@example.com"
          autoComplete="email"
        />
        {errors.email && <span className="auth-field__error">{errors.email}</span>}
      </label>

      <label className="auth-field">
        <span>Password</span>
        <div className="auth-field__password">
          <input
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="********"
            autoComplete="current-password"
          />
          <button
            type="button"
            className="auth-field__toggle"
            onClick={() => setShowPassword((value) => !value)}
            aria-label={showPassword ? 'Hide password' : 'Show password'}
          >
            <EyeIcon open={showPassword} />
          </button>
        </div>
        {errors.password && <span className="auth-field__error">{errors.password}</span>}
      </label>

      <div className="auth-form__row">
        <label className="auth-checkbox">
          <input
            type="checkbox"
            checked={rememberMe}
            onChange={(event) => setRememberMe(event.target.checked)}
          />
          <span>Remember me</span>
        </label>
        {/* Placeholder only; password recovery is not implemented yet */}
        <a className="auth-link" href="#" onClick={(event) => event.preventDefault()}>
          Forgot password?
        </a>
      </div>

      <button type="submit" className="auth-submit" disabled={isSubmitting}>
        {isSubmitting ? 'Signing in…' : 'Sign In'}
      </button>

      <div className="auth-divider">
        <span>or continue with</span>
      </div>

      <div className="auth-social">
        {/* TODO: wire up Google OAuth once backend support exists */}
        <button type="button" className="auth-social__btn" disabled title="Coming soon">
          <GoogleIcon />
          Continue with Google
        </button>
        {/* TODO: wire up Microsoft OAuth once backend support exists */}
        <button type="button" className="auth-social__btn" disabled title="Coming soon">
          <MicrosoftIcon />
          Continue with Microsoft
        </button>
      </div>
    </form>
  );
}

export default SignInForm;
