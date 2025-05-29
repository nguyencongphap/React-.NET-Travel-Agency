import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { Link } from "react-router";

const SignIn = () => {
  const handleSignIn = async () => {};

  // check className auth for how to set up bg
  return (
    <main className="auth">
      <section className="size-full glassmorphism flex-center px-6">
        <div className="sign-in-card">
          <header className="header">
            <Link to="/">
              <img
                src="/assets/icons/logo.svg"
                alt="logo"
                className="size-[30px]"
              />
            </Link>
            <h1 className="p-28-bold text-dark-100">Tourvisto</h1>
          </header>

          <article>
            <h2 className="p-28-semibold text-dark-100 text-center">
              Start Your Travel Journey
            </h2>
          </article>

          <ButtonComponent
            type="button"
            className="button-class !h-11 !w-full"
            onClick={handleSignIn}
          >
            <span className="p-18-semibold text-white">Sign In</span>
          </ButtonComponent>
        </div>
      </section>
    </main>
  );
};

export default SignIn;
