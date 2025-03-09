type User = {
  userName: string;
  email: string;
};

declare global {
  interface Window {
    __antiForgeryToken?: string;
    __themeKey: string;
    __user?: User;
    __model?: unknown;
    __viewData?: unknown;
  }
}
