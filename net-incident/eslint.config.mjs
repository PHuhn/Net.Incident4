import path from "node:path";
import { fileURLToPath } from "node:url";
import js from "@eslint/js";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

export default [{
    ignores: ["projects/**/*"],
}, ...compat.extends(
    "plugin:@angular-eslint/recommended",
    "plugin:@angular-eslint/template/process-inline-templates",
).map(config => ({
    ...config,
    files: ["**/*.ts"],
})), {
    files: ["**/*.ts"],

    languageOptions: {
        ecmaVersion: 5,
        sourceType: "script",

        parserOptions: {
            project: ["tsconfig.json", "e2e/tsconfig.json"],
            createDefaultProgram: true,
        },
    },

    rules: {
        "@angular-eslint/directive-selector": ["error", {
            type: "attribute",
            prefix: "app",
            style: "camelCase",
        }],

        "@angular-eslint/component-selector": ["error", {
            type: "element",
            prefix: "app",
            style: "kebab-case",
        }],

        "@angular-eslint/template/elements-content": "off",
    
        // Object.prototype method 'hasOwnProperty'
        "no-prototype-builtins": "off",
    
        "@typescript-eslint/no-inferrable-types": "off",

        // Components, Directives and Pipes should not opt out of standalone  @angular-eslint/prefer-standalone
        "@angular-eslint/prefer-standalone": "off",

        // Prefer using the inject() function over constructor parameter injection. Use Angular's migration schematic to automatically refactor
        // https://medium.com/ngconf/how-do-i-test-code-using-inject-e1278283f47c
        "@angular-eslint/prefer-inject": "off"

        /*
        // Object.prototype method 'hasOwnProperty'
        "no-prototype-builtins": "off",
        "@angular-eslint/elements-content": "off",
        "@angular-eslint/template/elements-content": "off",
        "@angular-eslint/template/elements-content": ["error", {
        allowList: ["pButton"]
        }],

        "@typescript-eslint/no-explicit-any": "off",
        */
    },
}, ...compat.extends("plugin:@angular-eslint/template/recommended").map(config => ({
    ...config,
    files: ["**/*.html"],
})), {
    files: ["**/*.html"],
    rules: {},
}];