import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { Link } from "react-router";
import { useForm, type SubmitHandler } from "react-hook-form";

type RegisterFormFields = {
  email: string;
  password: string;
  confirmPassword: string;
};

const Register = () => {
  const { register, handleSubmit } = useForm<RegisterFormFields>();

  const onSubmit: SubmitHandler<RegisterFormFields> = (data) => {
    console.log(data);
  };

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
            <form
              className="flex flex-col gap-2"
              onSubmit={handleSubmit(onSubmit)}
            >
              <input {...register("email")} type="text" placeholder="Email" />
              <input
                {...register("password")}
                type="passsword"
                placeholder="Password"
              />
              <input
                {...register("confirmPassword")}
                type="passsword"
                placeholder="Confirm Password"
              />
              <button
                type="submit"
                className="button-class !h-11 !w-full mt-[30px]"
              >
                <span className="p-18-semibold text-white">Register</span>
              </button>
            </form>
          </article>
        </div>
      </section>
    </main>
  );
};

export default Register;
