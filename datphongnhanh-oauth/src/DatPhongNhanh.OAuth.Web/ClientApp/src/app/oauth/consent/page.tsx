
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"
import { Check, Info, Shield, User, Mail, Calendar } from "lucide-react"


export default function ConsentPage() {
  return (
    <div className="flex h-screen w-full items-center justify-center bg-slate-50 p-4 dark:bg-slate-950">
      <Card className="w-full max-w-lg">
        <CardHeader className="space-y-1">
          <div className="flex items-center justify-center mb-4">
            <div className="h-16 w-16 rounded-full bg-primary/10 flex items-center justify-center">
              <Shield className="h-8 w-8 text-primary" />
            </div>
          </div>
          <CardTitle className="text-2xl font-bold text-center">App Permissions</CardTitle>
          <CardDescription className="text-center">
            <span className="font-medium">ExampleApp</span> would like to access your account
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-6">
          <div className="rounded-lg border bg-card p-4">
            <div className="flex items-start space-x-3">
              <Info className="h-5 w-5 text-muted-foreground mt-0.5" />
              <div className="space-y-1">
                <p className="text-sm font-medium leading-none">This will allow ExampleApp to:</p>
                <p className="text-sm text-muted-foreground">
                  These permissions will remain active until you revoke access in your account settings.
                </p>
              </div>
            </div>
          </div>

          <div className="space-y-4">
            <div className="flex items-start space-x-4">
              <div className="mt-0.5 bg-primary/10 p-1 rounded-full">
                <User className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="font-medium">View your basic profile</p>
                <p className="text-sm text-muted-foreground">Access to your name, username, and profile picture</p>
              </div>
            </div>

            <div className="flex items-start space-x-4">
              <div className="mt-0.5 bg-primary/10 p-1 rounded-full">
                <Mail className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="font-medium">View your email address</p>
                <p className="text-sm text-muted-foreground">Access to your primary email address</p>
              </div>
            </div>

            <div className="flex items-start space-x-4">
              <div className="mt-0.5 bg-primary/10 p-1 rounded-full">
                <Calendar className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="font-medium">View your activity history</p>
                <p className="text-sm text-muted-foreground">Access to your login history and account activity</p>
              </div>
            </div>
          </div>

          <div className="relative">
            <div className="absolute inset-0 flex items-center">
              <Separator />
            </div>
            <div className="relative flex justify-center text-xs uppercase">
              <span className="bg-card px-2 text-muted-foreground">Privacy Notice</span>
            </div>
          </div>

          <p className="text-sm text-muted-foreground">
            By approving, you allow this app to use your information in accordance with their
            <a href="#" className="font-medium text-primary underline-offset-4 hover:underline ml-1">
              privacy policy
            </a>{" "}
            and
            <a href="#" className="font-medium text-primary underline-offset-4 hover:underline ml-1">
              terms of service
            </a>
            .
          </p>
        </CardContent>
        <CardFooter className="flex flex-col space-y-2 sm:flex-row sm:justify-between sm:space-x-2 sm:space-y-0">
          <Button variant="outline" className="w-full sm:w-auto">
            Cancel
          </Button>
          <Button className="w-full sm:w-auto">
            <Check className="mr-2 h-4 w-4" />
            Allow Access
          </Button>
        </CardFooter>
      </Card>
    </div>
  )
}

