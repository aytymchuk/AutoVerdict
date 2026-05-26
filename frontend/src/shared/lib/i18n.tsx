/* eslint-disable react-refresh/only-export-components */
import { useState, createContext, useContext } from 'react';
import type { ReactNode } from 'react';

export type Language = 'en' | 'pl' | 'uk';

export const translations = {
  en: {
    // Chat page
    title: 'Vehicle Investigation Assistant',
    description:
      'Ask questions about vehicle history, CEPiK records, or upload documents for analysis.',
    signInToContinue: 'Please sign in to continue',
    signIn: 'Sign In',
    chatPlaceholder: 'Ask AutoVerdikt...',
    send: 'Send',

    // Landing — nav
    landing_nav_howItWorks: 'How it works',
    landing_nav_joinWhitelist: 'Join the whitelist →',

    // Landing — hero
    landing_hero_badge: 'In development · Launch Q3 2026',
    landing_hero_headline: "Don't get fooled when buying a used car.",
    landing_hero_description:
      'AutoVerdikt analyzes the listing, checks the history, and tells you what to watch out for — before you pay.',
    landing_hero_cta_primary: 'Join the whitelist →',
    landing_hero_cta_secondary: 'See how it works',
    landing_hero_note: 'Takes 30 seconds to register · First 200 get bonus credits',

    // Landing — mock card
    landing_mock_label: 'Listing Analysis',
    landing_mock_risk_medium: 'Medium Risk',
    landing_mock_risk_high: 'High Risk',
    landing_mock_mileage_label: 'Mileage',
    landing_mock_mileage_value: 'Possible rollback (-40k km)',
    landing_mock_history_label: 'History',
    landing_mock_history_value: 'Total loss (USA)',
    landing_mock_processing: 'Processing data...',

    // Landing — how it works
    landing_hiw_title: 'How it works',
    landing_hiw_description:
      'Get a complete picture of the risks associated with the selected vehicle in just three steps.',
    landing_hiw_step1_title: '1. Share the listing data',
    landing_hiw_step1_desc:
      'Copy text from the listing or enter basic car details manually. You can also add photos or documents.',
    landing_hiw_step2_title: '2. AI analyzes in seconds',
    landing_hiw_step2_desc:
      'We check the specs, assess the risk, and compare with the market. Your data is never stored.',
    landing_hiw_step3_title: '3. Get a report and questions',
    landing_hiw_step3_desc:
      'Risk rating, list of issues, and ready-made questions to ask the seller before you buy.',

    // Landing — data control
    landing_data_title: 'Your data, your control',
    landing_data_description: 'Choose the method that works best for you. We handle the rest.',
    landing_data_card1_badge: 'Manual',
    landing_data_card1_title: 'Enter manually',
    landing_data_card1_desc: 'Type in the VIN and basic car details to run our core analysis engine.',
    landing_data_card2_badge: 'Fast',
    landing_data_card2_title: 'Paste the listing text',
    landing_data_card2_desc:
      'Copy the description from any marketplace. We extract the relevant details.',
    landing_data_card3_badge: 'Pro',
    landing_data_card3_title: 'Add photos or documents',
    landing_data_card3_desc: 'Upload images. Our AI extracts specs and flags hidden visual clues.',

    // Landing — features
    landing_features_title: 'Tools for market advantage',
    landing_features_card1_label: 'Risk Analysis',
    landing_features_card1_title: 'Risk rating',
    landing_features_card1_desc:
      "Clear, data-backed assessment of the vehicle's risk profile based on history and specs.",
    landing_features_card1_tag1: 'Accident History',
    landing_features_card1_tag2: 'Price Valuation',
    landing_features_card1_tag2_value: '-12% vs Market',
    landing_features_card2_title: 'Questions for the seller',
    landing_features_card2_desc:
      'Generated list of targeted questions to expose inconsistencies and negotiate better.',
    landing_features_card3_title: 'Photo analysis',
    landing_features_card3_desc:
      'AI scans listing photos for hidden damage, mismatched parts, and wear indicators.',
    landing_features_card4_title: 'Document data',
    landing_features_card4_desc:
      'Extract maintenance records and verify paperwork authenticity instantly.',
    landing_features_card5_title: 'Market comparison',
    landing_features_card5_desc:
      'See how the car stacks up against active listings and historical sales data for true valuation.',
    landing_features_card6_title: 'Chat with your data',
    landing_features_card6_desc:
      'Ask specific questions about the analyzed car history and get instant AI answers.',

    // Landing — stats
    landing_stats_label1: 'Data Stored',
    landing_stats_label2: 'Objective Analysis',
    landing_stats_label3: 'Instant Reports',
    landing_stats_note: 'Your data is analyzed in memory and instantly discarded.',

    // Landing — CTA
    landing_cta_title: 'Join the closed beta',
    landing_cta_button: 'Join the whitelist →',
    landing_cta_disclaimer:
      '*By joining, you agree to our Terms of Service and Privacy Policy.',

    // Landing — footer
    landing_footer_privacy: 'Privacy Policy',
    landing_footer_terms: 'Terms of Service',
    landing_footer_howItWorks: 'How it works',
    landing_footer_copyright: '© 2026 AutoVerdikt. All rights reserved.',
    landing_footer_language: 'English',
  },

  pl: {
    // Chat page
    title: 'Asystent ds. Badania Pojazdów',
    description:
      'Zadawaj pytania dotyczące historii pojazdu, rekordów CEPiK lub przesyłaj dokumenty do analizy.',
    signInToContinue: 'Zaloguj się, aby kontynuować',
    signIn: 'Zaloguj Się',
    chatPlaceholder: 'Zapytaj AutoVerdikt...',
    send: 'Wyślij',

    // Landing — nav
    landing_nav_howItWorks: 'Jak to działa',
    landing_nav_joinWhitelist: 'Dołącz do listy →',

    // Landing — hero
    landing_hero_badge: 'W trakcie rozwoju · Start III kw. 2026',
    landing_hero_headline: 'Nie daj się oszukać przy zakupie używanego auta.',
    landing_hero_description:
      'AutoVerdikt analizuje ogłoszenie, sprawdza historię i mówi, na co uważać — zanim zapłacisz.',
    landing_hero_cta_primary: 'Dołącz do listy →',
    landing_hero_cta_secondary: 'Jak to działa',
    landing_hero_note: 'Rejestracja trwa 30 sekund · Pierwsze 200 osób dostaje bonusowe kredyty',

    // Landing — mock card
    landing_mock_label: 'Analiza ogłoszenia',
    landing_mock_risk_medium: 'Średnie ryzyko',
    landing_mock_risk_high: 'Wysokie ryzyko',
    landing_mock_mileage_label: 'Przebieg',
    landing_mock_mileage_value: 'Możliwy rollback (-40 tys. km)',
    landing_mock_history_label: 'Historia',
    landing_mock_history_value: 'Całkowita szkoda (USA)',
    landing_mock_processing: 'Przetwarzanie danych...',

    // Landing — how it works
    landing_hiw_title: 'Jak to działa',
    landing_hiw_description:
      'Poznaj pełny obraz ryzyka związanego z wybranym pojazdem w zaledwie trzech krokach.',
    landing_hiw_step1_title: '1. Podaj dane ogłoszenia',
    landing_hiw_step1_desc:
      'Skopiuj tekst ogłoszenia lub wprowadź podstawowe dane ręcznie. Możesz też dodać zdjęcia lub dokumenty.',
    landing_hiw_step2_title: '2. AI analizuje w sekundy',
    landing_hiw_step2_desc:
      'Sprawdzamy specyfikację, oceniamy ryzyko i porównujemy z rynkiem. Twoje dane nie są przechowywane.',
    landing_hiw_step3_title: '3. Otrzymaj raport i pytania',
    landing_hiw_step3_desc:
      'Ocena ryzyka, lista problemów i gotowe pytania do sprzedawcy przed zakupem.',

    // Landing — data control
    landing_data_title: 'Twoje dane, Twoja kontrola',
    landing_data_description: 'Wybierz metodę, która Ci odpowiada. Resztą zajmiemy się my.',
    landing_data_card1_badge: 'Ręcznie',
    landing_data_card1_title: 'Wprowadź ręcznie',
    landing_data_card1_desc:
      'Wpisz numer VIN i podstawowe dane auta, aby uruchomić naszą analizę.',
    landing_data_card2_badge: 'Szybko',
    landing_data_card2_title: 'Wklej tekst ogłoszenia',
    landing_data_card2_desc:
      'Skopiuj opis z dowolnego portalu. Wyodrębniamy istotne informacje.',
    landing_data_card3_badge: 'Pro',
    landing_data_card3_title: 'Dodaj zdjęcia lub dokumenty',
    landing_data_card3_desc:
      'Prześlij zdjęcia. Nasza AI wyciąga dane i wykrywa ukryte sygnały ostrzegawcze.',

    // Landing — features
    landing_features_title: 'Narzędzia przewagi rynkowej',
    landing_features_card1_label: 'Analiza ryzyka',
    landing_features_card1_title: 'Ocena ryzyka',
    landing_features_card1_desc:
      'Jasna, oparta na danych ocena profilu ryzyka pojazdu na podstawie historii i specyfikacji.',
    landing_features_card1_tag1: 'Historia wypadków',
    landing_features_card1_tag2: 'Wycena ceny',
    landing_features_card1_tag2_value: '-12% wobec rynku',
    landing_features_card2_title: 'Pytania do sprzedawcy',
    landing_features_card2_desc:
      'Wygenerowana lista celnych pytań, które ujawniają niespójności i pomagają negocjować.',
    landing_features_card3_title: 'Analiza zdjęć',
    landing_features_card3_desc:
      'AI skanuje zdjęcia z ogłoszenia w poszukiwaniu ukrytych uszkodzeń, niezgodnych części i śladów zużycia.',
    landing_features_card4_title: 'Dane z dokumentów',
    landing_features_card4_desc:
      'Wyciągaj historię serwisową i natychmiast weryfikuj autentyczność dokumentów.',
    landing_features_card5_title: 'Porównanie z rynkiem',
    landing_features_card5_desc:
      'Sprawdź, jak auto wypada na tle aktywnych ofert i danych historycznych dla realnej wyceny.',
    landing_features_card6_title: 'Rozmawiaj ze swoimi danymi',
    landing_features_card6_desc:
      'Zadawaj szczegółowe pytania o analizowaną historię auta i uzyskuj natychmiastowe odpowiedzi AI.',

    // Landing — stats
    landing_stats_label1: 'Przechowywanych danych',
    landing_stats_label2: 'Obiektywna analiza',
    landing_stats_label3: 'Natychmiastowe raporty',
    landing_stats_note: 'Twoje dane są analizowane w pamięci i natychmiast usuwane.',

    // Landing — CTA
    landing_cta_title: 'Dołącz do zamkniętej bety',
    landing_cta_button: 'Dołącz do listy →',
    landing_cta_disclaimer:
      '*Dołączając, akceptujesz nasz Regulamin i Politykę prywatności.',

    // Landing — footer
    landing_footer_privacy: 'Polityka prywatności',
    landing_footer_terms: 'Regulamin',
    landing_footer_howItWorks: 'Jak to działa',
    landing_footer_copyright: '© 2026 AutoVerdikt. Wszelkie prawa zastrzeżone.',
    landing_footer_language: 'Polski',
  },

  uk: {
    // Chat page
    title: 'Асистент з розслідування транспортних засобів',
    description:
      'Ставте запитання про історію автомобіля, записи CEPiK або завантажуйте документи для аналізу.',
    signInToContinue: 'Будь ласка, увійдіть, щоб продовжити',
    signIn: 'Увійти',
    chatPlaceholder: 'Запитати AutoVerdikt...',
    send: 'Надіслати',

    // Landing — nav
    landing_nav_howItWorks: 'Як це працює',
    landing_nav_joinWhitelist: 'Приєднатися до списку →',

    // Landing — hero
    landing_hero_badge: 'У розробці · Запуск III кв. 2026',
    landing_hero_headline: 'Не дайте себе обдурити при купівлі вживаного авто.',
    landing_hero_description:
      'AutoVerdikt аналізує оголошення, перевіряє історію та розповідає, на що звернути увагу — перш ніж ви заплатите.',
    landing_hero_cta_primary: 'Приєднатися до списку →',
    landing_hero_cta_secondary: 'Як це працює',
    landing_hero_note: 'Реєстрація займає 30 секунд · Перші 200 отримають бонусні кредити',

    // Landing — mock card
    landing_mock_label: 'Аналіз оголошення',
    landing_mock_risk_medium: 'Середній ризик',
    landing_mock_risk_high: 'Високий ризик',
    landing_mock_mileage_label: 'Пробіг',
    landing_mock_mileage_value: 'Можливий відкат (-40 тис. км)',
    landing_mock_history_label: 'Історія',
    landing_mock_history_value: 'Повна втрата (США)',
    landing_mock_processing: 'Обробка даних...',

    // Landing — how it works
    landing_hiw_title: 'Як це працює',
    landing_hiw_description:
      "Отримайте повну картину ризиків, пов'язаних із вибраним автомобілем, лише за три кроки.",
    landing_hiw_step1_title: '1. Поділіться даними оголошення',
    landing_hiw_step1_desc:
      'Скопіюйте текст оголошення або введіть основні дані вручну. Також можна додати фото або документи.',
    landing_hiw_step2_title: '2. AI аналізує за секунди',
    landing_hiw_step2_desc:
      'Ми перевіряємо специфікацію, оцінюємо ризик і порівнюємо з ринком. Ваші дані не зберігаються.',
    landing_hiw_step3_title: '3. Отримайте звіт та запитання',
    landing_hiw_step3_desc:
      'Оцінка ризику, перелік проблем та готові запитання до продавця до покупки.',

    // Landing — data control
    landing_data_title: 'Ваші дані, ваш контроль',
    landing_data_description: 'Оберіть зручний для вас спосіб. Ми подбаємо про решту.',
    landing_data_card1_badge: 'Вручну',
    landing_data_card1_title: 'Введіть вручну',
    landing_data_card1_desc: 'Введіть VIN та основні дані авто, щоб запустити наш аналіз.',
    landing_data_card2_badge: 'Швидко',
    landing_data_card2_title: 'Вставте текст оголошення',
    landing_data_card2_desc:
      'Скопіюйте опис з будь-якого майданчика. Ми витягнемо потрібні деталі.',
    landing_data_card3_badge: 'Pro',
    landing_data_card3_title: 'Додайте фото або документи',
    landing_data_card3_desc:
      'Завантажте фото. Наш AI витягує дані та виявляє приховані застережні сигнали.',

    // Landing — features
    landing_features_title: 'Інструменти ринкової переваги',
    landing_features_card1_label: 'Аналіз ризику',
    landing_features_card1_title: 'Оцінка ризику',
    landing_features_card1_desc:
      "Чітка, підкріплена даними оцінка профілю ризику автомобіля на основі історії та специфікацій.",
    landing_features_card1_tag1: 'Історія аварій',
    landing_features_card1_tag2: 'Оцінка ціни',
    landing_features_card1_tag2_value: '-12% від ринку',
    landing_features_card2_title: 'Запитання до продавця',
    landing_features_card2_desc:
      'Згенерований список цільових запитань для виявлення невідповідностей і кращого торгу.',
    landing_features_card3_title: 'Аналіз фото',
    landing_features_card3_desc:
      'AI сканує фото з оголошення на приховані пошкодження, невідповідні деталі та ознаки зношеності.',
    landing_features_card4_title: 'Дані з документів',
    landing_features_card4_desc:
      'Витягуйте записи технічного обслуговування та миттєво перевіряйте автентичність документів.',
    landing_features_card5_title: 'Порівняння з ринком',
    landing_features_card5_desc:
      'Подивіться, як авто виглядає на тлі активних оголошень та історичних даних продажів для реальної оцінки вартості.',
    landing_features_card6_title: 'Спілкуйтеся зі своїми даними',
    landing_features_card6_desc:
      'Ставте конкретні запитання про проаналізовану історію авто та отримуйте миттєві відповіді AI.',

    // Landing — stats
    landing_stats_label1: 'Збережених даних',
    landing_stats_label2: "Об'єктивний аналіз",
    landing_stats_label3: 'Миттєві звіти',
    landing_stats_note: 'Ваші дані аналізуються в памʼяті та негайно видаляються.',

    // Landing — CTA
    landing_cta_title: 'Приєднайтесь до закритої бети',
    landing_cta_button: 'Приєднатися до списку →',
    landing_cta_disclaimer:
      "*Приєднуючись, ви погоджуєтесь з нашими Умовами використання та Політикою конфіденційності.",

    // Landing — footer
    landing_footer_privacy: 'Політика конфіденційності',
    landing_footer_terms: 'Умови використання',
    landing_footer_howItWorks: 'Як це працює',
    landing_footer_copyright: '© 2026 AutoVerdikt. Всі права захищені.',
    landing_footer_language: 'Українська',
  },
} as const;

export type TranslationKey = keyof typeof translations['en'];

function getInitialLanguage(): Language {
  if (typeof window === 'undefined') return 'en';

  const browserLang = navigator.language.split('-')[0].toLowerCase();

  // Russian is NEVER supported. If browser language is 'ru', fallback to 'en'.
  if (browserLang === 'ru') {
    return 'en';
  }

  if (browserLang === 'pl') return 'pl';
  if (browserLang === 'uk') return 'uk';

  return 'en';
}

interface I18nContextType {
  language: Language;
  setLanguage: (lang: Language) => void;
  t: (key: TranslationKey) => string;
}

export const I18nContext = createContext<I18nContextType | null>(null);

export function useTranslation() {
  const context = useContext(I18nContext);
  if (!context) {
    const lang = getInitialLanguage();
    return {
      language: lang,
      setLanguage: () => {},
      t: (key: TranslationKey) => translations[lang][key] || translations['en'][key],
    };
  }
  return context;
}

interface I18nProviderProps {
  children: ReactNode;
}

export function I18nProvider({ children }: I18nProviderProps) {
  const [language, setLanguageState] = useState<Language>(getInitialLanguage);

  const setLanguage = (lang: Language) => {
    // Safeguard to ensure Russian is never selected
    if ((lang as string) === 'ru') {
      setLanguageState('en');
    } else {
      setLanguageState(lang);
    }
  };

  const t = (key: TranslationKey): string => {
    return translations[language][key] || translations['en'][key];
  };

  return (
    <I18nContext.Provider value={{ language, setLanguage, t }}>
      {children}
    </I18nContext.Provider>
  );
}
