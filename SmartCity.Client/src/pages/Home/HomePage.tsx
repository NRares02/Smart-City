import PublicNavbar from '../../components/home/PublicNavbar';
import HeroSection from '../../components/home/HeroSection';
import IntroSection from '../../components/home/IntroSection';
import FeatureSection from '../../components/home/FeatureSection';
import HowItWorks from '../../components/home/HowItWorks';
import CityStats from '../../components/home/CityStats';
import ExploreCitySection from '../../components/home/ExploreCitySection';
import NewsPreview from '../../components/home/NewsPreview';
import CitizenCTA from '../../components/home/CitizenCTA';
import FinalCta from '../../components/home/FinalCta';
import PublicFooter from '../../components/home/PublicFooter';
import './HomePage.css';

function HomePage() {
  return (
    <div className="home-page">
      <PublicNavbar />
      <HeroSection />
      <IntroSection />
      <FeatureSection />
      <HowItWorks />
      <CityStats />
      <ExploreCitySection />
      <NewsPreview />
      <CitizenCTA />
      <FinalCta />
      <PublicFooter />
    </div>
  );
}

export default HomePage;
