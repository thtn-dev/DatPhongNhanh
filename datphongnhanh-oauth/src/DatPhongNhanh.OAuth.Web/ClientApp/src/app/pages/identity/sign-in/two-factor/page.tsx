
import TwoFactorForm from "@/components/forms/two-factor-form"


export default function TwoFactorPage() {
  return (
    <div className="container flex h-screen w-screen flex-col items-center justify-center">
      <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
        <div className="flex flex-col space-y-2 text-center">
          <h1 className="text-2xl font-semibold tracking-tight">Two-Factor Authentication</h1>
          <p className="text-sm text-muted-foreground">Enter the verification code sent to your device</p>
        </div>
        <TwoFactorForm />
        <p className="px-8 text-center text-sm text-muted-foreground">
          <a href="/login" className="underline underline-offset-4 hover:text-primary">
            Back to login
          </a>
        </p>
      </div>
    </div>
  )
}

