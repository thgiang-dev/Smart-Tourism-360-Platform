/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          light: '#ccfbf1',
          DEFAULT: '#0f766e',
        },
        secondary: '#f59e0b',
        accent: '#ea580c',
      }
    },
  },
  plugins: [],
}
