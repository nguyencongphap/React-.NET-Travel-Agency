import { useForm, type SubmitHandler } from "react-hook-form";
import { Link, redirect, useNavigate } from "react-router";
import { GetCurrentUser } from "~/api/authApi";
import Axios from "~/api/axios";
import useAuth from "~/hooks/useAuth";
import useAxiosPrivate from "~/hooks/useAxiosPrivate";

export async function clientLoader() {
  try {
    const user = await GetCurrentUser();

    if (user.id) return redirect("/");
  } catch (error) {
    console.log("Error fetching user", error);
  }
}

type SignInFormFields = {
  username: string;
  password: string;
};

const SignIn = () => {
  const navigate = useNavigate();
  const { auth, setAuth } = useAuth();

  const axiosPrivate = useAxiosPrivate();

  const { register, handleSubmit } = useForm<SignInFormFields>();

  const onSubmit: SubmitHandler<SignInFormFields> = async (data) => {
    const { username, password } = data;

    try {
      const resp = await Axios.post<{ accessToken: string }>("/login", {
        username,
        password,
      });

      if (!username) navigate("/");

      setAuth({ username, accessToken: resp?.data?.accessToken });

      console.log("resp", resp);
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

            {/* TODO: DEL LATER */}
            {/* <button
              onClick={async () => {
                const resp = await axiosPrivate.get("checkAuth");
                console.log("check-auth resp", resp);
              }}
            >
              Send Authorized Request
            </button>

            <button
              onClick={async () => {
                await axiosPrivate.get("/movies");
              }}
            >
              Test
            </button> */}
          </article>
        </div>
      </section>
    </main>
  );
};

export default SignIn;
