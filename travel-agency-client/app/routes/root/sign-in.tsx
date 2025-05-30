import axios from "~/api/axios";
import { useForm, type SubmitHandler } from "react-hook-form";
import { Link } from "react-router";
import { useContext } from "react";
import AuthContext from "~/context/AuthProvider";

type SignInFormFields = {
  username: string;
  password: string;
};

const SignIn = () => {
  const { auth, setAuth } = useContext(AuthContext);

  const { register, handleSubmit } = useForm<SignInFormFields>();

  const onSubmit: SubmitHandler<SignInFormFields> = async (data) => {
    // TODO: DEL LATER
    console.log(data);

    const { username, password } = data;

    try {
      const resp = await axios.post("/login", { username, password });
      const accessToken = resp?.data?.accessToken;
      setAuth({ username, password, accessToken });

      console.log("resp", resp);
    } catch (error) {
      console.log("error", error);
    }
  };

  // TODO: DEL LATER
  console.log("auth", auth);
  console.log("setAuth", setAuth);

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
              <input
                {...register("username")}
                type="text"
                placeholder="Email"
              />
              <input
                {...register("password")}
                type="password"
                placeholder="Password"
              />
              <button
                type="submit"
                className="button-class !h-11 !w-full mt-[30px]"
              >
                <span className="p-18-semibold text-white">Sign In</span>
              </button>
            </form>
            <p>Need an Account?</p>
            <Link to="/register">Sign Up</Link>
          </article>
        </div>
      </section>
    </main>
  );
};

export default SignIn;
