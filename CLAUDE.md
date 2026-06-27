 # CLAUDE.md — Portfolio Project Memory
 
 > این فایل حافظه‌ی فشرده‌ی پروژه برای Claude است.
 > قبل از هر تغییر، این فایل و در صورت نیاز فایل `.mdc` مرتبط را بخوان.
 
 ## Tech Stack
 
 Blazor WebAssembly (.NET 10, `net10.0`) — client-side SPA بدون backend.
 استایل: pure vanilla CSS با CSS custom properties (بدون Tailwind/SCSS).
 دو پالت رنگی (A=Studio Slate, B=Engine Forge) با `[data-direction]`.
 فونت: Geist + JetBrains Mono + Instrument Serif. آیکون: Font Awesome 6.
 دیپلوی: GitHub Actions → GitHub Pages. PWA با service worker.
 
 ## Folder Structure
 
 ```
 Portfolio/src/Portfolio/
 ├── Pages/              # صفحات routable — هر page: .razor + .razor.cs
 │   ├── Index.razor      # صفحه‌ی اصلی (compose sections)
 │   ├── GitHub.razor      # صفحه‌ی repositories
 │   └── 404.razor         # Not Found
 ├── Layout/             # MainLayout + NavMenu — هرکدام .razor + .razor.cs
 ├── Components/
 │   ├── Footer.razor
 │   ├── Sections/       # section های صفحه‌ی اصلی — inline @code
 │   │   ├── AboutSection.razor
 │   │   ├── ExperienceSection.razor
 │   │   ├── ProjectSection.razor
 │   │   ├── EducationSection.razor
 │   │   ├── SkillsSection.razor
 │   │   ├── InterestsSection.razor
 │   │   ├── RepositoriesSection.razor
 │   │   └── CTASection.razor
 │   └── Navigation/     # AnchorNavigation (scroll handling)
 ├── Services/           # BuildInfo.cs (static)
 ├── wwwroot/
 │   ├── css/styles.css   # CSS variables, palettes
 │   ├── css/app.css      # All component styles (minified)
 │   ├── js/scripts.js    # JS interop functions
 │   └── assets/          # Images, icons
 ├── Program.cs          # DI setup (HttpClient only)
 └── _Imports.razor       # Global usings
 ```
 
 ## Key Conventions (one-liners)
 
 - **Code-behind:** Pages و Layout → `.razor.cs` جداگانه. Sections → inline `@code`.
 - **DI:** فقط `@inject` در `.razor`، نه `[Inject]` در code-behind.
 - **HttpClient:** `GetFromJsonAsync<T>` + try/catch + null = loading state.
 - **Data models:** درون همان component تعریف شوند (record برای immutable, class برای API).
 - **داده‌ی استاتیک:** `private static readonly` arrays/records.
 - **CSS رنگ‌ها:** فقط `var(--bg)`, `var(--fg)`, `var(--accent)` و مشابه. هرگز hard-code.
 - **CSS نام‌گذاری:** کوتاه، flat، lowercase با `-`. بدون BEM.
 - **Sections:** `<section class="sec" id="...">` + `<div class="sec-marker">` + `.reveal` class.
 - **JS interop:** توابع در `scripts.js`، PascalCase، فراخوانی با `InvokeVoidAsync`.
 - **Routing:** `@page` + `<HeadContent>` با canonical link + `<PageTitle>`.
 - **Private fields:** با `_` شروع شوند (الگوی غالب جدید).
 - **Namespace:** `Portfolio.{Folder}` مطابق ساختار فولدر.
 - **Responsive:** breakpoints ثابت: 1200px, 900px, 600px, 480px.
 - **Scroll reveal:** عناصر اصلی section ها `class="... reveal"` بگیرند.
 
 ## ارجاع به قوانین تفصیلی
 
 | موضوع | فایل |
 |---|---|
 | نمای کلی پروژه و قوانین حیاتی | `.cursor/rules/000-project-overview.mdc` |
 | ساخت کامپوننت، code-behind، lifecycle، DI | `.cursor/rules/010-blazor-components.mdc` |
 | استایل، رنگ‌ها، تایپوگرافی، responsive | `.cursor/rules/020-styling-css.mdc` |
 | سرویس‌ها، HttpClient، مدل‌های داده | `.cursor/rules/030-services-and-data.mdc` |
 | نام‌گذاری فایل، کلاس، namespace، CSS | `.cursor/rules/040-naming-conventions.mdc` |
 
 ## دستورالعمل برای Claude
 
 1. **قبل از هر تغییر** در کامپوننت‌ها/استایل/سرویس‌ها، اول این فایل را بخوان. اگر جزئیات بیشتری لازم بود، فایل `.mdc` مرتبط (از جدول بالا) را باز کن.
 2. **هرگز** از کاربر نپرس "معماری پروژه چیست؟" یا "از چه فریمورکی استفاده می‌کنید؟" — جواب همینجاست.
 3. **هرگز** Tailwind، SCSS، یا پکیج NuGet جدید اضافه نکن مگر صراحتاً درخواست شود.
 4. **همیشه** هر دو پالت رنگی را رعایت کن (Direction A و B).
 5. **همیشه** الگوهای موجود پروژه را دنبال کن — نه best practice عمومی.
