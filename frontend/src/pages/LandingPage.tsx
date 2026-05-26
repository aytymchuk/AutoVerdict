import { useTranslation } from '../shared/lib/i18n';
import type { Language } from '../shared/lib/i18n';

const LANGUAGES: Language[] = ['en', 'pl', 'uk'];

export function LandingPage() {
  const { t, language, setLanguage } = useTranslation();

  function cycleLanguage() {
    const idx = LANGUAGES.indexOf(language);
    setLanguage(LANGUAGES[(idx + 1) % LANGUAGES.length]);
  }

  return (
    <div className="bg-surface-container-lowest text-text-primary antialiased selection:bg-primary-container selection:text-surface-container-lowest">
      {/* TopNavBar */}
      <nav className="fixed top-0 w-full z-50 bg-background/80 backdrop-blur-md border-b border-outline-variant/20">
        <div className="max-w-[1280px] mx-auto px-gutter h-20 flex items-center justify-between">
          <div className="flex items-center gap-xl">
            <a className="font-headline-md text-[36px] leading-[1.3] font-semibold text-on-surface tracking-tight" href="#">
              AutoVerdikt
            </a>
            <div className="hidden md:flex gap-lg">
              <a className="text-on-surface-variant hover:text-primary transition-colors text-[16px]" href="#how-it-works">
                {t('landing_nav_howItWorks')}
              </a>
            </div>
          </div>
          <div className="flex items-center gap-lg">
            <button
              aria-label="Switch language"
              title={t('landing_footer_language')}
              onClick={cycleLanguage}
              className="text-on-surface-variant hover:text-primary transition-colors flex items-center justify-center gap-1.5 px-3 h-10 rounded-full hover:bg-surface-container-highest text-[13px] font-medium"
            >
              <span className="material-symbols-outlined text-[20px]">language</span>
              <span className="hidden sm:inline uppercase tracking-wider text-[11px]">{language}</span>
            </button>
            <a
              className="bg-primary-container text-surface-container-lowest text-[16px] font-bold py-3 px-6 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)]"
              href="/auth"
            >
              {t('landing_nav_joinWhitelist')}
            </a>
          </div>
        </div>
      </nav>

      <main className="pt-20">
        {/* Hero Section */}
        <section className="max-w-[1280px] mx-auto px-gutter py-xxl lg:py-[120px] grid grid-cols-1 lg:grid-cols-12 gap-xl items-center min-h-[921px]">
          <div className="lg:col-span-7 flex flex-col gap-lg">
            <div>
              <span className="inline-block bg-surface-alt border border-border-subtle text-text-secondary font-[JetBrains_Mono] text-[12px] px-3 py-1 rounded-full mb-4">
                {t('landing_hero_badge')}
              </span>
              <h1 className="font-[Fraunces] text-[40px] lg:text-[64px] leading-[1.1] font-bold tracking-[-0.02em] text-on-surface">
                {t('landing_hero_headline')}
              </h1>
            </div>
            <p className="font-[DM_Sans] text-[20px] leading-[1.6] text-on-surface-variant max-w-2xl">
              {t('landing_hero_description')}
            </p>
            <div className="mt-md flex flex-col gap-4">
              <div className="flex flex-wrap gap-md">
                <a
                  className="inline-flex items-center justify-center bg-primary-container text-surface-container-lowest text-[16px] font-bold py-4 px-8 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)]"
                  href="/auth"
                >
                  {t('landing_hero_cta_primary')}
                </a>
                <a
                  className="inline-flex items-center justify-center bg-transparent text-primary-container border border-primary-container text-[16px] font-bold py-4 px-8 rounded-full hover:bg-surface-container-highest transition-all duration-300"
                  href="#how-it-works"
                >
                  {t('landing_hero_cta_secondary')}{' '}
                  <span className="material-symbols-outlined ml-2 text-[20px]">arrow_forward</span>
                </a>
              </div>
              <p className="text-[14px] text-text-secondary flex items-center gap-2">
                <span className="text-[16px] text-primary-container">✦</span>
                {t('landing_hero_note')}
              </p>
            </div>
          </div>

          <div className="lg:col-span-5 relative">
            {/* Mock UI Card */}
            <div className="bg-surface rounded-xl border border-border-subtle p-lg shadow-[0_8px_32px_rgba(0,0,0,0.4)] inner-glow relative overflow-hidden">
              <div className="absolute inset-0 pointer-events-none">
                <div className="absolute inset-0 bg-gradient-to-br from-surface-alt/50 to-transparent"></div>
              </div>
              <div className="relative z-10 flex flex-col gap-md">
                <div className="flex justify-between items-start border-b border-border-subtle pb-md">
                  <div>
                    <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary uppercase tracking-widest mb-1">
                      {t('landing_mock_label')}
                    </div>
                    <h3 className="text-[20px] font-bold text-on-surface">Volkswagen Golf 2018</h3>
                    <div className="font-[JetBrains_Mono] text-[14px] text-on-surface-variant mt-1">
                      VIN: WVWZZZAUZJW******
                    </div>
                  </div>
                  <div className="bg-risk-medium/15 text-risk-medium px-3 py-1.5 rounded-full flex items-center gap-2 border border-risk-medium/20">
                    <span className="material-symbols-outlined fill text-[16px]">warning</span>
                    <span className="font-[DM_Sans] text-[12px] uppercase tracking-[0.1em] font-bold">
                      {t('landing_mock_risk_medium')}
                    </span>
                  </div>
                </div>

                <div className="space-y-4 py-2">
                  <div className="relative h-2 bg-surface-container-highest rounded-full overflow-hidden">
                    <div className="absolute top-0 left-0 h-full w-[65%] bg-risk-medium rounded-full"></div>
                    <div className="absolute top-0 left-0 h-full w-full scan-line"></div>
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div className="bg-surface-alt p-3 rounded-lg border border-border-subtle inner-glow flex items-start gap-3">
                      <div className="w-8 h-8 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center shrink-0">
                        <span className="material-symbols-outlined text-[18px]">history</span>
                      </div>
                      <div>
                        <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary">
                          {t('landing_mock_mileage_label')}
                        </div>
                        <div className="text-[14px] text-on-surface mt-1">
                          {t('landing_mock_mileage_value')}
                        </div>
                      </div>
                    </div>
                    <div className="bg-surface-alt p-3 rounded-lg border border-border-subtle inner-glow flex items-start gap-3">
                      <div className="w-8 h-8 rounded-full bg-risk-high/15 text-risk-high flex items-center justify-center shrink-0">
                        <span className="material-symbols-outlined text-[18px]">car_crash</span>
                      </div>
                      <div>
                        <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary">
                          {t('landing_mock_history_label')}
                        </div>
                        <div className="text-[14px] text-on-surface mt-1">
                          {t('landing_mock_history_value')}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="mt-2 pt-md border-t border-border-subtle flex justify-between items-center">
                  <div className="flex items-center gap-2 text-text-secondary font-[JetBrains_Mono] text-[12px]">
                    <span className="material-symbols-outlined text-[16px] animate-spin">sync</span>
                    {t('landing_mock_processing')}
                  </div>
                  <div className="font-[JetBrains_Mono] text-[14px] font-medium text-primary-container">
                    65%
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* How It Works */}
        <section className="bg-surface py-xxl border-y border-outline-variant/10" id="how-it-works">
          <div className="max-w-[1280px] mx-auto px-gutter">
            <div className="text-center mb-xl">
              <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface">
                {t('landing_hiw_title')}
              </h2>
              <p className="text-[20px] leading-[1.6] text-on-surface-variant mt-4 max-w-2xl mx-auto">
                {t('landing_hiw_description')}
              </p>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-lg relative">
              {/* Connector Line (Desktop) */}
              <div className="hidden md:block absolute top-1/2 left-[16%] right-[16%] h-[1px] border-t border-dashed border-outline-variant/30 -translate-y-1/2 z-0"></div>

              {/* Step 1 */}
              <div className="relative z-10 bg-surface-container-lowest p-lg rounded-xl border border-border-subtle inner-glow flex flex-col items-center text-center">
                <div className="absolute -top-6 -left-4 font-[Fraunces] text-[80px] text-surface-container-highest font-bold opacity-50 select-none">
                  01
                </div>
                <div className="w-16 h-16 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md border border-primary-container/20">
                  <span className="material-symbols-outlined text-[32px]">content_paste</span>
                </div>
                <h3 className="font-[Fraunces] text-[24px] font-semibold text-on-surface mb-2">
                  {t('landing_hiw_step1_title')}
                </h3>
                <p className="text-[14px] text-on-surface-variant">{t('landing_hiw_step1_desc')}</p>
              </div>

              {/* Step 2 */}
              <div className="relative z-10 bg-surface-container-lowest p-lg rounded-xl border border-border-subtle inner-glow flex flex-col items-center text-center">
                <div className="absolute -top-6 -left-4 font-[Fraunces] text-[80px] text-surface-container-highest font-bold opacity-50 select-none">
                  02
                </div>
                <div className="w-16 h-16 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md border border-primary-container/20">
                  <span className="material-symbols-outlined text-[32px]">memory</span>
                </div>
                <h3 className="font-[Fraunces] text-[24px] font-semibold text-on-surface mb-2">
                  {t('landing_hiw_step2_title')}
                </h3>
                <p className="text-[14px] text-on-surface-variant">{t('landing_hiw_step2_desc')}</p>
              </div>

              {/* Step 3 */}
              <div className="relative z-10 bg-surface-container-lowest p-lg rounded-xl border border-border-subtle inner-glow flex flex-col items-center text-center">
                <div className="absolute -top-6 -left-4 font-[Fraunces] text-[80px] text-surface-container-highest font-bold opacity-50 select-none">
                  03
                </div>
                <div className="w-16 h-16 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md border border-primary-container/20">
                  <span className="material-symbols-outlined text-[32px]">analytics</span>
                </div>
                <h3 className="font-[Fraunces] text-[24px] font-semibold text-on-surface mb-2">
                  {t('landing_hiw_step3_title')}
                </h3>
                <p className="text-[14px] text-on-surface-variant">{t('landing_hiw_step3_desc')}</p>
              </div>
            </div>
          </div>
        </section>

        {/* Data Control Section */}
        <section className="max-w-[1280px] mx-auto px-gutter py-xxl border-b border-outline-variant/10">
          <div className="text-center mb-xl">
            <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface">
              {t('landing_data_title')}
            </h2>
            <p className="text-[20px] leading-[1.6] text-on-surface-variant mt-4 max-w-2xl mx-auto">
              {t('landing_data_description')}
            </p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-lg">
            {/* Card 1 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-surface-alt transition-all duration-300 flex flex-col">
              <div className="flex justify-between items-start mb-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center">
                  <span className="material-symbols-outlined text-[24px]">edit_document</span>
                </div>
                <span className="font-[DM_Sans] text-[10px] bg-surface-container-highest px-2 py-1 rounded text-text-secondary uppercase tracking-widest">
                  {t('landing_data_card1_badge')}
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                {t('landing_data_card1_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">{t('landing_data_card1_desc')}</p>
            </div>

            {/* Card 2 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-surface-alt transition-all duration-300 flex flex-col">
              <div className="flex justify-between items-start mb-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center">
                  <span className="material-symbols-outlined text-[24px]">content_copy</span>
                </div>
                <span className="font-[DM_Sans] text-[10px] bg-primary-container/20 text-primary-container px-2 py-1 rounded uppercase tracking-widest">
                  {t('landing_data_card2_badge')}
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                {t('landing_data_card2_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">{t('landing_data_card2_desc')}</p>
            </div>

            {/* Card 3 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-surface-alt transition-all duration-300 flex flex-col">
              <div className="flex justify-between items-start mb-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center">
                  <span className="material-symbols-outlined text-[24px]">photo_camera</span>
                </div>
                <span className="font-[DM_Sans] text-[10px] bg-secondary-container/20 text-secondary-container px-2 py-1 rounded uppercase tracking-widest">
                  {t('landing_data_card3_badge')}
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                {t('landing_data_card3_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">{t('landing_data_card3_desc')}</p>
            </div>
          </div>
        </section>

        {/* Features Bento */}
        <section className="max-w-[1280px] mx-auto px-gutter py-xxl">
          <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface mb-xl text-center">
            {t('landing_features_title')}
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-lg">
            {/* Feature Card 1 — wide */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col lg:col-span-2 relative overflow-hidden">
              <div className="relative z-10">
                <span className="font-[DM_Sans] text-[12px] text-primary-container tracking-widest uppercase mb-sm block">
                  {t('landing_features_card1_label')}
                </span>
                <h3 className="font-[Fraunces] text-[36px] leading-[1.3] font-semibold text-on-surface mb-md max-w-md">
                  {t('landing_features_card1_title')}
                </h3>
                <p className="text-[14px] text-on-surface-variant">
                  {t('landing_features_card1_desc')}
                </p>
              </div>
              <div className="mt-xl grid grid-cols-2 gap-sm relative z-10">
                <div className="bg-surface-alt border border-border-subtle p-sm rounded-lg">
                  <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary mb-xs">
                    {t('landing_features_card1_tag1')}
                  </div>
                  <div className="flex items-center gap-xs">
                    <span className="bg-risk-high/15 text-risk-high px-2 py-1 rounded-full flex items-center text-xs font-bold gap-1 border border-risk-high/20">
                      <span className="material-symbols-outlined text-[14px]">error</span>{' '}
                      {t('landing_mock_risk_high')}
                    </span>
                  </div>
                </div>
                <div className="bg-surface-alt border border-border-subtle p-sm rounded-lg">
                  <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary mb-xs">
                    {t('landing_features_card1_tag2')}
                  </div>
                  <div className="font-[JetBrains_Mono] text-[14px] font-medium text-on-surface">
                    {t('landing_features_card1_tag2_value')}
                  </div>
                </div>
              </div>
              <div className="absolute right-0 top-0 w-64 h-64 bg-primary-container/5 rounded-full blur-3xl -mr-12 -mt-12"></div>
            </div>

            {/* Feature Card 2 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">forum</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">
                {t('landing_features_card2_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                {t('landing_features_card2_desc')}
              </p>
            </div>

            {/* Feature Card 3 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">photo_library</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">
                {t('landing_features_card3_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                {t('landing_features_card3_desc')}
              </p>
            </div>

            {/* Feature Card 4 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">description</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">
                {t('landing_features_card4_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                {t('landing_features_card4_desc')}
              </p>
            </div>

            {/* Feature Card 5 — wide */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center lg:col-span-2">
              <div className="flex items-start gap-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center shrink-0 group-hover:bg-primary-container/20">
                  <span className="material-symbols-outlined text-[24px]">query_stats</span>
                </div>
                <div>
                  <h3 className="text-[20px] font-bold text-on-surface mb-2">
                    {t('landing_features_card5_title')}
                  </h3>
                  <p className="text-[14px] text-on-surface-variant max-w-lg">
                    {t('landing_features_card5_desc')}
                  </p>
                </div>
              </div>
            </div>

            {/* Feature Card 6 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">chat_bubble</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">
                {t('landing_features_card6_title')}
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                {t('landing_features_card6_desc')}
              </p>
            </div>
          </div>
        </section>

        {/* Stats / Trust Section */}
        <section className="bg-surface-alt border-y border-outline-variant/10 py-xl">
          <div className="max-w-[1280px] mx-auto px-gutter text-center">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-lg mb-8">
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">0</div>
                <div className="text-[16px] text-text-secondary">{t('landing_stats_label1')}</div>
              </div>
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">100%</div>
                <div className="text-[16px] text-text-secondary">{t('landing_stats_label2')}</div>
              </div>
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">24/7</div>
                <div className="text-[16px] text-text-secondary">{t('landing_stats_label3')}</div>
              </div>
            </div>
            <p className="font-[JetBrains_Mono] text-[12px] text-text-secondary flex items-center justify-center gap-2">
              <span className="material-symbols-outlined text-[16px]">lock</span>
              {t('landing_stats_note')}
            </p>
          </div>
        </section>

        {/* Final CTA Section */}
        <section className="py-xxl relative" id="join-beta">
          <div className="max-w-[1280px] mx-auto px-gutter relative z-10 text-center">
            <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface mb-8">
              {t('landing_cta_title')}
            </h2>
            <a
              className="inline-flex items-center justify-center bg-primary-container text-surface-container-lowest text-[16px] font-bold py-4 px-12 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)] mb-4"
              href="/auth"
            >
              {t('landing_cta_button')}
            </a>
            <p className="font-[JetBrains_Mono] text-[12px] text-text-secondary">
              {t('landing_cta_disclaimer')}
            </p>
          </div>
        </section>
      </main>

      {/* Footer */}
      <footer className="w-full py-xl bg-surface-container-lowest border-t border-outline-variant/10">
        <div className="max-w-[1280px] mx-auto px-gutter flex flex-col gap-md">
          <div className="flex flex-col md:flex-row justify-between items-center gap-6">
            <div className="font-[Fraunces] text-[36px] leading-[1.3] font-semibold text-on-surface">
              AutoVerdikt
            </div>
            <div className="flex flex-wrap justify-center gap-6">
              <a className="text-[14px] text-on-surface-variant hover:text-primary transition-colors" href="#">
                {t('landing_footer_privacy')}
              </a>
              <a className="text-[14px] text-on-surface-variant hover:text-primary transition-colors" href="#">
                {t('landing_footer_terms')}
              </a>
              <a className="text-[14px] text-on-surface-variant hover:text-primary transition-colors" href="#how-it-works">
                {t('landing_footer_howItWorks')}
              </a>
            </div>
          </div>
          <div className="flex flex-col md:flex-row justify-between items-center gap-4 pt-6 border-t border-border-subtle">
            <div className="text-[14px] text-text-secondary">{t('landing_footer_copyright')}</div>
            <button
              onClick={cycleLanguage}
              className="flex items-center gap-2 text-text-secondary hover:text-primary transition-colors text-[14px]"
            >
              <span className="material-symbols-outlined text-[16px]">language</span>
              <span>{t('landing_footer_language')}</span>
            </button>
          </div>
        </div>
      </footer>
    </div>
  );
}
