import { defineConfig } from "vitest/config";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

// https://vitest.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],

  test: {
    environment: "jsdom",
    globals: true,

    reporters: ["verbose", "junit"],
    outputFile: {
      junit: "./test-results/junit.xml",
    },

    coverage: {
      provider: "v8",
      reporter: ["text", "lcov", "cobertura"],
      reportsDirectory: "./test-results/coverage",

      thresholds: {
        lines: 60,
        branches: 60,
        functions: 60,
        statements: 60,
      },

      exclude: [
        "node_modules/**",
        "dist/**",
        "**/*.d.ts",
        "**/*.config.*",
        "src/main.tsx",
      ],
    },
  },
});
