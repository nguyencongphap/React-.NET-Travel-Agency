import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { Link } from "react-router";
import { useForm, type SubmitHandler } from "react-hook-form";
import { useState } from "react";
import Axios from "~/api/axios";

type RegisterFormFields = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
};

const Register = () => {
  const { register, handleSubmit } = useForm<RegisterFormFields>();

  const [isSuccessRegister, setIsSuccessRegister] = useState(false);

  const onSubmit: SubmitHandler<RegisterFormFields> = async (data) => {
    const { firstName, lastName, email, password } = data;

    try {
      const resp = await Axios.post("Auth/register", {
        firstName,
        lastName,
        email,
        password,
      });

      console.log("resp", resp);
      setIsSuccessRegister(true);
    } catch (error) {
      console.log("error", error);
    }
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

          {isSuccessRegister ? (
            <article>
              <h2 className="p-28-semibold text-dark-100 text-center">
                Success!
              </h2>
              <p className="flex justify-center mt-3">
                <Link to="/sign-in">Sign in</Link>
              </p>
            </article>
          ) : (
            <article>
              <h2 className="p-28-semibold text-dark-100 text-center">
                Start Your Travel Journey
              </h2>
              <form
                className="flex flex-col gap-2"
                onSubmit={handleSubmit(onSubmit)}
              >
                <input
                  {...register("firstName")}
                  type="text"
                  placeholder="First Name"
                />
                <input
                  {...register("lastName")}
                  type="text"
                  placeholder="Last Name"
                />
                <input {...register("email")} type="text" placeholder="Email" />
                <input
                  {...register("password")}
                  type="password"
                  placeholder="Password"
                />
                <input
                  {...register("confirmPassword")}
                  type="password"
                  placeholder="Confirm Password"
                />
                <button
                  type="submit"
                  className="button-class !h-11 !w-full mt-[30px]"
                >
                  <span className="p-18-semibold text-white">Register</span>
                </button>
              </form>
              <p>Already register?</p>
              <Link to="/sign-in">Sign in</Link>
            </article>
          )}
        </div>
      </section>
    </main>
  );
};

export default Register;
