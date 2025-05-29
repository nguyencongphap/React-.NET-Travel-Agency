import { reactRouter } from "@react-router/dev/vite";
import tailwindcss from "@tailwindcss/vite";
import { defineConfig } from "vite";
import tsconfigPaths from "vite-tsconfig-paths";

export default defineConfig({
  plugins: [tailwindcss(), reactRouter(), tsconfigPaths()],
  // prevent dependencies from being used for Server Side Rendering allowing us to use Syncfusion without any issue
  ssr: {
    noExternal: [/@syncfusion/],
  },
});
