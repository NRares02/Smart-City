import { useState, type ChangeEvent, type FormEvent } from 'react';
import { getAuthErrorMessage, register } from '../../services/authService';
import EyeIcon from './EyeIcon';

interface SignUpFormProps {
  onRegistered: (email: string) => void;
}

interface FormState {
  name: string;
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
}

interface FormErrors {
  name?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
  form?: string;
}

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const MIN_PASSWORD_LENGTH = 6;

const initialState: FormState = {
  name: '',
  email: '',
  phoneNumber: '',
  password: '',
  confirmPassword: '',
};

function SignUpForm({ onRegistered }: SignUpFormProps) {
  const [values, setValues] = useState<FormState>(initialState);
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState<FormErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  function handleChange(field: keyof FormState) {
    return (event: ChangeEvent<HTMLInputElement>) => {
      setValues((previous) => ({ ...previous, [field]: event.target.value }));
    };
  }

  function validate(): boolean {
    const nextErrors: FormErrors = {};
    if (!values.name.trim()) {
      nextErrors.name = 'Name is required.';
    }
    if (!values.email.trim()) {
      nextErrors.email = 'Email is required.';
    } else if (!EMAIL_PATTERN.test(values.email)) {
      nextErrors.email = 'Enter a valid email address.';
    }
    if (!values.password) {
      nextErrors.password = 'Password is required.';
    } else if (values.password.length < MIN_PASSWORD_LENGTH) {
      nextErrors.password = `Password must be at least ${MIN_PASSWORD_LENGTH} characters.`;
    }
    if (values.confirmPassword !== values.password) {
      nextErrors.confirmPassword = 'Passwords do not match.';
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
      await register({
        name: values.name.trim(),
        email: values.email.trim(),
        password: values.password,
        phoneNumber: values.phoneNumber.trim() || undefined,
      });
      onRegistered(values.email.trim());
    } catch (error) {
      setErrors({ form: getAuthErrorMessage(error, 'Unable to create your account. Please try again.') });
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="auth-form" onSubmit={handleSubmit} noValidate>
      {errors.form && <p className="auth-form__error auth-form__error--general">{errors.form}</p>}

      <label className="auth-field">
        <span>Full name</span>
        <input value={values.name} onChange={handleChange('name')} placeholder="Jane Doe" autoComplete="name" />
        {errors.name && <span className="auth-field__error">{errors.name}</span>}
      </label>

      <label className="auth-field">
        <span>Email</span>
        <input
          type="email"
          value={values.email}
          onChange={handleChange('email')}
          placeholder="you@example.com"
          autoComplete="email"
        />
        {errors.email && <span className="auth-field__error">{errors.email}</span>}
      </label>

      <label className="auth-field">
        <span>
          Phone number <em>(optional)</em>
        </span>
        <input
          value={values.phoneNumber}
          onChange={handleChange('phoneNumber')}
          placeholder="+1 555 123 4567"
          autoComplete="tel"
        />
      </label>

      <label className="auth-field">
        <span>Password</span>
        <div className="auth-field__password">
          <input
            type={showPassword ? 'text' : 'password'}
            value={values.password}
            onChange={handleChange('password')}
            placeholder="********"
            autoComplete="new-password"
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

      <label className="auth-field">
        <span>Confirm password</span>
        <input
          type={showPassword ? 'text' : 'password'}
          value={values.confirmPassword}
          onChange={handleChange('confirmPassword')}
          placeholder="********"
          autoComplete="new-password"
        />
        {errors.confirmPassword && <span className="auth-field__error">{errors.confirmPassword}</span>}
      </label>

      <button type="submit" className="auth-submit" disabled={isSubmitting}>
        {isSubmitting ? 'Creating account…' : 'Sign Up'}
      </button>
    </form>
  );
}

export default SignUpForm;
