import type { ReactNode } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from '../lib/i18n';
import { useCycleLanguage } from '../lib/i18n/useCycleLanguage';

interface PageShellProps {
  children: ReactNode;
}

export function PageShell({ children }: PageShellProps) {
  const { t } = useTranslation();
  const { language, cycleLanguage } = useCycleLanguage();

  return (
    <div className="flex flex-col min-h-screen bg-surface-container-lowest text-on-surface font-body-md antialiased overflow-x-hidden">
      {/* Fixed Top NavBar */}
      <header className="bg-background/80 backdrop-blur-md fixed top-0 w-full z-50 border-b border-outline-variant/20">
        <div className="max-w-[1280px] mx-auto px-gutter h-20 flex items-center justify-between">
          <Link
            to="/"
            className="font-headline-md text-[24px] font-semibold text-on-surface tracking-tight hover:text-primary transition-colors"
          >
            AutoVerdikt
          </Link>
          <button
            type="button"
            onClick={cycleLanguage}
            aria-label={t('common_switch_language')}
            className="flex items-center justify-center p-sm rounded-full text-on-surface-variant hover:text-primary transition-colors hover:bg-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-risk-medium"
          >
            <span className="material-symbols-outlined text-[20px]">language</span>
            <span className="ml-sm font-label-caps text-label-caps uppercase tracking-wider">
              {language}
            </span>
          </button>
        </div>
      </header>

      {/* Main Content */}
      <main className="flex-grow pt-20 flex items-center justify-center relative p-gutter md:py-xxl">
        {/* Ambient background glow */}
        <div className="absolute inset-0 pointer-events-none overflow-hidden flex justify-center items-center opacity-30">
          <div className="w-[800px] h-[800px] bg-risk-medium/5 rounded-full blur-[120px]" />
        </div>

        <div className="w-full max-w-3xl z-10">{children}</div>
      </main>

      {/* Footer */}
      <footer className="w-full py-lg bg-surface-container-lowest border-t border-outline-variant/10">
        <div className="max-w-[1280px] mx-auto px-gutter flex flex-col gap-md md:flex-row md:items-center md:justify-between">
          <div className="flex flex-col md:flex-row md:items-center gap-md">
            <span className="font-headline-md text-[24px] font-semibold text-on-surface">
              AutoVerdikt
            </span>
            <span className="hidden md:block w-1 h-1 rounded-full bg-outline-variant" />
            <p className="font-body-sm text-[14px] text-on-surface-variant">
              {t('footer_copyright').replace('{year}', String(new Date().getFullYear()))}
            </p>
          </div>
          <nav aria-label={t('common_footer_nav')}>
            <ul className="flex flex-wrap gap-md md:gap-lg">
              <li>
                <Link
                  to="/privacy"
                  className="font-body-sm text-[14px] text-on-surface-variant hover:text-primary transition-colors duration-300"
                >
                  {t('footer_privacy')}
                </Link>
              </li>
              <li>
                <Link
                  to="/terms"
                  className="font-body-sm text-[14px] text-on-surface-variant hover:text-primary transition-colors duration-300"
                >
                  {t('footer_terms')}
                </Link>
              </li>
            </ul>
          </nav>
        </div>
      </footer>
    </div>
  );
}
