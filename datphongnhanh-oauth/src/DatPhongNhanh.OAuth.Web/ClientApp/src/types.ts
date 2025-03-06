type User = {
  userName: string;
  email: string;
};

declare global {
  interface Window {
    __antiForgeryToken?: string;
    __dautruongUiThemeKey: string;
    __user?: User;
  }
}
