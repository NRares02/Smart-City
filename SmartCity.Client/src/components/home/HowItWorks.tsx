const STEPS = [
  {
    number: '01',
    title: 'Create your account',
    description: 'Sign up in seconds and join your local Smart City community.',
  },
  {
    number: '02',
    title: 'Report or explore',
    description: 'Submit a report about a local issue or explore city services and points of interest.',
  },
  {
    number: '03',
    title: 'Track updates and stay informed',
    description: 'Follow the progress of your reports and get news and announcements as they happen.',
  },
];

function HowItWorks() {
  return (
    <section className="how-it-works">
      <div className="how-it-works__header">
        <p className="features__eyebrow">Getting Started</p>
        <h2>How Smart City works</h2>
      </div>

      <div className="how-it-works__grid">
        {STEPS.map((step) => (
          <div className="how-step" key={step.number}>
            <span className="how-step__number">{step.number}</span>
            <h3>{step.title}</h3>
            <p>{step.description}</p>
          </div>
        ))}
      </div>
    </section>
  );
}

export default HowItWorks;
