# Design System Document: Banco Argentum Digital Identity

## 1. Overview & Creative North Star: "The Architectural Vault"

To design for a modern banking institution is to balance the gravity of tradition with the fluid speed of technology. Our Creative North Star is **"The Architectural Vault."** 

This design system rejects the "flatness" of generic fintech. Instead, it treats the digital interface as a series of physical, premium spaces. We break the template look through **Intentional Asymmetry**—using wide margins and off-center focal points—and **Tonal Layering**. By eschewing traditional borders in favor of overlapping surfaces and light-refracting glass effects, we create an environment that feels both immutably secure and technologically advanced.

---

## 2. Colors & Surface Philosophy

The palette is anchored in deep authority and punctuated by "Trustworthy Cyan" to guide the eye.

### The "No-Line" Rule
Standard 1px borders are strictly prohibited for sectioning. Structural integrity must be achieved through background shifts. For example, a card utilizing `surface-container-lowest` (#ffffff) should sit atop a `surface-container-low` (#f2f4f6) section. This creates a "soft edge" that feels integrated rather than walled off.

### Surface Hierarchy & Nesting
Treat the UI as a physical stack. Each layer must move closer to or further from the light source:
*   **Base Layer:** `surface` (#f7f9fb)
*   **Sectioning:** `surface-container-low` (#f2f4f6)
*   **Interactive Cards:** `surface-container-lowest` (#ffffff)
*   **High-Priority Overlays:** `surface-bright` (#f7f9fb) with Glassmorphism.

### The "Glass & Gradient" Rule
To elevate the "Technologically Advanced" vibe, use `surface-tint` (#3a5f94) at 10% opacity with a `backdrop-blur` of 20px for floating navigation or modal headers. 

**Signature Texture:** Main Action Areas should utilize a subtle linear gradient:
*   **Start:** `primary` (#001e40) 
*   **End:** `primary-container` (#003366) at a 135-degree angle.

---

## 3. Typography: The Editorial Scale

We use **Inter** as our typographic backbone. It is a typeface of precision and clarity. To achieve a "High-End Editorial" look, we utilize extreme contrast in our scale.

*   **Display (lg/md/sm):** Used for big-picture financial health or hero statements. Use `primary` color with `-0.02em` letter spacing to feel "locked-in" and authoritative.
*   **Headline (lg/md):** Used for page titles. Pair with generous `16 (4rem)` top spacing to create a "gallery" feel.
*   **Body (lg/md):** All transactional data and descriptions. Use `on-surface-variant` (#43474f) for secondary body text to reduce visual noise.
*   **Labels (md/sm):** Reserved for micro-data (e.g., timestamps, card numbers). Use `medium` weight to maintain legibility against light-grey backgrounds.

---

## 4. Elevation & Depth

We convey hierarchy through **Tonal Layering** rather than structural lines.

*   **The Layering Principle:** Depth is achieved by stacking. Place a `surface-container-lowest` card on a `surface-container-low` background. This creates a natural "lift" without the "cheapness" of heavy shadows.
*   **Ambient Shadows:** For floating elements (Modals, Dropdowns), use a "tinted" shadow. Instead of black, use `on-surface` (#191c1e) at 6% opacity with a `48px` blur and `12px` Y-offset. This mimics natural light reflecting off deep navy surfaces.
*   **The Ghost Border:** If a border is required for accessibility (e.g., Input fields), use `outline-variant` (#c3c6d1) at **20% opacity**. Never use 100% opaque borders.
*   **Glassmorphism:** Use `primary-container` with 80% opacity and a backdrop blur for high-end mobile navigation bars.

---

## 5. Components

### Buttons
*   **Primary:** Background: `primary-container` (#003366); Text: `on-primary` (#ffffff). Shape: `md` (0.375rem). Use a subtle inner-glow (top-down) for a "pressed-metal" feel.
*   **Secondary:** Background: `secondary-fixed` (#cce5ff); Text: `on-secondary-fixed` (#001d31). 
*   **Tertiary:** No background. `on-primary-fixed-variant` (#1f477b) text.

### Input Fields
*   **Surface:** `surface-container-highest` (#e0e3e5).
*   **State:** On focus, transition the background to `surface-container-lowest` and add a `2px` "Ghost Border" of `secondary` (#006398).
*   **Error:** Background remains, but `outline` shifts to `error` (#ba1a1a).

### Cards & Financial Lists
*   **The Separation Rule:** Forbid divider lines. Separate list items using `spacing-4` (1rem) of vertical white space or by alternating background tones between `surface-container-lowest` and `surface-container-low`.
*   **Micro-Interactions:** Cards should subtly scale (1.02x) on hover, with the ambient shadow increasing in spread but decreasing in opacity.

### Additional Banking Components
*   **Transaction Chips:** Use `secondary-container` (#00a9fd) with 15% opacity for "Pending" states and `primary-container` for "Settled" states.
*   **Balance Display:** Utilize `display-md` for the integer and `title-md` for the decimals to create a sophisticated typographic hierarchy.

---

## 6. Do’s and Don’ts

### Do
*   **Do** use asymmetrical padding (e.g., `spacing-12` on the left, `spacing-8` on the right) for hero sections to create a custom, high-end feel.
*   **Do** use `secondary` (#006398) sparingly as a "laser pointer" to guide users to the "Deposit" or "Transfer" actions.
*   **Do** embrace negative space. If a screen feels "empty," it’s likely working; banking is stressful, the UI shouldn't be.

### Don’t
*   **Don’t** use pure black (#000000) for text. Always use `on-surface` (#191c1e).
*   **Don’t** use "Drop Shadows" on buttons. Use tonal shifts.
*   **Don’t** use rounded corners larger than `xl` (0.75rem) for main containers. Excessive roundness (pills) can make a professional bank look like a social media app. Keep the `md` (0.375rem) radius for a disciplined, architectural feel.