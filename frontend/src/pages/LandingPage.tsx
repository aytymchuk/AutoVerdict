export function LandingPage() {
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
              <a
                className="text-on-surface-variant hover:text-primary transition-colors text-[16px]"
                href="#how-it-works"
              >
                How it works
              </a>
            </div>
          </div>
          <div className="flex items-center gap-lg">
            <button
              aria-label="Language"
              className="text-on-surface-variant hover:text-primary transition-colors flex items-center justify-center w-10 h-10 rounded-full"
            >
              <span className="material-symbols-outlined">language</span>
            </button>
            <a
              className="bg-primary-container text-surface-container-lowest text-[16px] font-bold py-3 px-6 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)]"
              href="/auth"
            >
              Join the whitelist →
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
                In development · Launch Q3 2026
              </span>
              <h1 className="font-[Fraunces] text-[40px] lg:text-[64px] leading-[1.1] font-bold tracking-[-0.02em] text-on-surface">
                Don't get fooled when buying a used car.
              </h1>
            </div>
            <p className="font-[DM_Sans] text-[20px] leading-[1.6] text-on-surface-variant max-w-2xl">
              AutoVerdikt analyzes the listing, checks the history, and tells you what to watch out
              for — before you pay.
            </p>
            <div className="mt-md flex flex-col gap-4">
              <div className="flex flex-wrap gap-md">
                <a
                  className="inline-flex items-center justify-center bg-primary-container text-surface-container-lowest text-[16px] font-bold py-4 px-8 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)]"
                  href="/auth"
                >
                  Join the whitelist →
                </a>
                <a
                  className="inline-flex items-center justify-center bg-transparent text-primary-container border border-primary-container text-[16px] font-bold py-4 px-8 rounded-full hover:bg-surface-container-highest transition-all duration-300"
                  href="#how-it-works"
                >
                  See how it works{' '}
                  <span className="material-symbols-outlined ml-2 text-[20px]">arrow_forward</span>
                </a>
              </div>
              <p className="text-[14px] text-text-secondary flex items-center gap-2">
                <span className="text-[16px] text-primary-container">✦</span>
                Takes 30 seconds to register · First 200 get bonus credits
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
                      Listing Analysis
                    </div>
                    <h3 className="text-[20px] font-bold text-on-surface">Volkswagen Golf 2018</h3>
                    <div className="font-[JetBrains_Mono] text-[14px] text-on-surface-variant mt-1">
                      VIN: WVWZZZAUZJW******
                    </div>
                  </div>
                  <div className="bg-risk-medium/15 text-risk-medium px-3 py-1.5 rounded-full flex items-center gap-2 border border-risk-medium/20">
                    <span className="material-symbols-outlined fill text-[16px]">warning</span>
                    <span className="font-[DM_Sans] text-[12px] uppercase tracking-[0.1em] font-bold">
                      Medium Risk
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
                          Mileage
                        </div>
                        <div className="text-[14px] text-on-surface mt-1">
                          Possible rollback (-40k km)
                        </div>
                      </div>
                    </div>
                    <div className="bg-surface-alt p-3 rounded-lg border border-border-subtle inner-glow flex items-start gap-3">
                      <div className="w-8 h-8 rounded-full bg-risk-high/15 text-risk-high flex items-center justify-center shrink-0">
                        <span className="material-symbols-outlined text-[18px]">car_crash</span>
                      </div>
                      <div>
                        <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary">
                          History
                        </div>
                        <div className="text-[14px] text-on-surface mt-1">Total loss (USA)</div>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="mt-2 pt-md border-t border-border-subtle flex justify-between items-center">
                  <div className="flex items-center gap-2 text-text-secondary font-[JetBrains_Mono] text-[12px]">
                    <span className="material-symbols-outlined text-[16px] animate-spin">sync</span>
                    Processing data...
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
                How it works
              </h2>
              <p className="text-[20px] leading-[1.6] text-on-surface-variant mt-4 max-w-2xl mx-auto">
                Get a complete picture of the risks associated with the selected vehicle in just
                three steps.
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
                  1. Share the listing data
                </h3>
                <p className="text-[14px] text-on-surface-variant">
                  Copy text from the listing or enter basic car details manually. You can also add
                  photos or documents.
                </p>
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
                  2. AI analyzes in seconds
                </h3>
                <p className="text-[14px] text-on-surface-variant">
                  We check the specs, assess the risk, and compare with the market. Your data is
                  never stored.
                </p>
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
                  3. Get a report and questions
                </h3>
                <p className="text-[14px] text-on-surface-variant">
                  Risk rating, list of issues, and ready-made questions to ask the seller before you
                  buy.
                </p>
              </div>
            </div>
          </div>
        </section>

        {/* Data Control Section */}
        <section className="max-w-[1280px] mx-auto px-gutter py-xxl border-b border-outline-variant/10">
          <div className="text-center mb-xl">
            <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface">
              Your data, your control
            </h2>
            <p className="text-[20px] leading-[1.6] text-on-surface-variant mt-4 max-w-2xl mx-auto">
              Choose the method that works best for you. We handle the rest.
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
                  Manual
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                Enter manually
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                Type in the VIN and basic car details to run our core analysis engine.
              </p>
            </div>

            {/* Card 2 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-surface-alt transition-all duration-300 flex flex-col">
              <div className="flex justify-between items-start mb-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center">
                  <span className="material-symbols-outlined text-[24px]">content_copy</span>
                </div>
                <span className="font-[DM_Sans] text-[10px] bg-primary-container/20 text-primary-container px-2 py-1 rounded uppercase tracking-widest">
                  Fast
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                Paste the listing text
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                Copy the description from any marketplace. We extract the relevant details.
              </p>
            </div>

            {/* Card 3 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-surface-alt transition-all duration-300 flex flex-col">
              <div className="flex justify-between items-start mb-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center">
                  <span className="material-symbols-outlined text-[24px]">photo_camera</span>
                </div>
                <span className="font-[DM_Sans] text-[10px] bg-secondary-container/20 text-secondary-container px-2 py-1 rounded uppercase tracking-widest">
                  Pro
                </span>
              </div>
              <h3 className="font-[Fraunces] text-[20px] font-semibold text-on-surface mb-2">
                Add photos or documents
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                Upload images. Our AI extracts specs and flags hidden visual clues.
              </p>
            </div>
          </div>
        </section>

        {/* Features Bento */}
        <section className="max-w-[1280px] mx-auto px-gutter py-xxl">
          <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface mb-xl text-center">
            Tools for market advantage
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-lg">
            {/* Feature Card 1 — wide */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col lg:col-span-2 relative overflow-hidden">
              <div className="relative z-10">
                <span className="font-[DM_Sans] text-[12px] text-primary-container tracking-widest uppercase mb-sm block">
                  Risk Analysis
                </span>
                <h3 className="font-[Fraunces] text-[36px] leading-[1.3] font-semibold text-on-surface mb-md max-w-md">
                  Risk rating
                </h3>
                <p className="text-[14px] text-on-surface-variant">
                  Clear, data-backed assessment of the vehicle's risk profile based on history and
                  specs.
                </p>
              </div>
              <div className="mt-xl grid grid-cols-2 gap-sm relative z-10">
                <div className="bg-surface-alt border border-border-subtle p-sm rounded-lg">
                  <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary mb-xs">
                    Accident History
                  </div>
                  <div className="flex items-center gap-xs">
                    <span className="bg-risk-high/15 text-risk-high px-2 py-1 rounded-full flex items-center text-xs font-bold gap-1 border border-risk-high/20">
                      <span className="material-symbols-outlined text-[14px]">error</span> High Risk
                    </span>
                  </div>
                </div>
                <div className="bg-surface-alt border border-border-subtle p-sm rounded-lg">
                  <div className="font-[JetBrains_Mono] text-[12px] text-text-secondary mb-xs">
                    Price Valuation
                  </div>
                  <div className="font-[JetBrains_Mono] text-[14px] font-medium text-on-surface">
                    -12% vs Market
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
                Questions for the seller
              </h3>
              <p className="text-[14px] text-on-surface-variant">
                Generated list of targeted questions to expose inconsistencies and negotiate better.
              </p>
            </div>

            {/* Feature Card 3 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">photo_library</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">Photo analysis</h3>
              <p className="text-[14px] text-on-surface-variant">
                AI scans listing photos for hidden damage, mismatched parts, and wear indicators.
              </p>
            </div>

            {/* Feature Card 4 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">description</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">Document data</h3>
              <p className="text-[14px] text-on-surface-variant">
                Extract maintenance records and verify paperwork authenticity instantly.
              </p>
            </div>

            {/* Feature Card 5 — wide */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center lg:col-span-2">
              <div className="flex items-start gap-md">
                <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center shrink-0 group-hover:bg-primary-container/20">
                  <span className="material-symbols-outlined text-[24px]">query_stats</span>
                </div>
                <div>
                  <h3 className="text-[20px] font-bold text-on-surface mb-2">Market comparison</h3>
                  <p className="text-[14px] text-on-surface-variant max-w-lg">
                    See how the car stacks up against active listings and historical sales data for
                    true valuation.
                  </p>
                </div>
              </div>
            </div>

            {/* Feature Card 6 */}
            <div className="bg-surface p-lg rounded-xl border border-border-subtle inner-glow hover:bg-[#1A2030] hover:border-l-4 hover:border-l-primary-container transition-all duration-300 group flex flex-col justify-center">
              <div className="w-12 h-12 rounded-full bg-primary-container/15 text-primary-container flex items-center justify-center mb-md group-hover:bg-primary-container/20">
                <span className="material-symbols-outlined text-[24px]">chat_bubble</span>
              </div>
              <h3 className="text-[20px] font-bold text-on-surface mb-2">Chat with your data</h3>
              <p className="text-[14px] text-on-surface-variant">
                Ask specific questions about the analyzed car history and get instant AI answers.
              </p>
            </div>
          </div>
        </section>

        {/* Stats / Trust Section */}
        <section className="bg-surface-alt border-y border-outline-variant/10 py-xl">
          <div className="max-w-[1280px] mx-auto px-gutter text-center">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-lg mb-8">
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">
                  0
                </div>
                <div className="text-[16px] text-text-secondary">Data Stored</div>
              </div>
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">
                  100%
                </div>
                <div className="text-[16px] text-text-secondary">Objective Analysis</div>
              </div>
              <div>
                <div className="font-[Fraunces] text-[48px] font-bold text-primary-container mb-2">
                  24/7
                </div>
                <div className="text-[16px] text-text-secondary">Instant Reports</div>
              </div>
            </div>
            <p className="font-[JetBrains_Mono] text-[12px] text-text-secondary flex items-center justify-center gap-2">
              <span className="material-symbols-outlined text-[16px]">lock</span>
              Your data is analyzed in memory and instantly discarded.
            </p>
          </div>
        </section>

        {/* Final CTA Section */}
        <section className="py-xxl relative" id="join-beta">
          <div className="max-w-[1280px] mx-auto px-gutter relative z-10 text-center">
            <h2 className="font-[Fraunces] text-[48px] leading-[1.2] font-semibold text-on-surface mb-8">
              Join the closed beta
            </h2>
            <a
              className="inline-flex items-center justify-center bg-primary-container text-surface-container-lowest text-[16px] font-bold py-4 px-12 rounded-full hover:bg-primary transition-all duration-300 shadow-[inset_0_1px_0_0_rgba(255,255,255,0.2)] mb-4"
              href="/auth"
            >
              Join the whitelist →
            </a>
            <p className="font-[JetBrains_Mono] text-[12px] text-text-secondary">
              *By joining, you agree to our Terms of Service and Privacy Policy.
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
              <a
                className="text-[14px] text-on-surface-variant hover:text-primary transition-colors"
                href="#"
              >
                Privacy Policy
              </a>
              <a
                className="text-[14px] text-on-surface-variant hover:text-primary transition-colors"
                href="#"
              >
                Terms of Service
              </a>
              <a
                className="text-[14px] text-on-surface-variant hover:text-primary transition-colors"
                href="#how-it-works"
              >
                How it works
              </a>
            </div>
          </div>
          <div className="flex flex-col md:flex-row justify-between items-center gap-4 pt-6 border-t border-border-subtle">
            <div className="text-[14px] text-text-secondary">
              © 2026 AutoVerdikt. All rights reserved.
            </div>
            <div className="flex items-center gap-2 text-text-secondary text-[14px]">
              <span className="material-symbols-outlined text-[16px]">language</span>
              <span>English</span>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}
