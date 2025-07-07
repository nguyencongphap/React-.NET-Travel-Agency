import Header from "components/Header";
import {
  ColumnDirective,
  ColumnsDirective,
  GridComponent,
} from "@syncfusion/ej2-react-grids";
import { users } from "~/constants";
import { cn, formatDate } from "~/lib/utils";
import { ROLE_NORMALIZED_NAMES } from "~/types/roleNames";
import type { UserData } from "~/index";
import { getAllUsers } from "./all-users/allUsersApi";
import type { Route } from "./+types/all-users";

export const clientLoader = async () => {
  const res = await getAllUsers(10, 0);
  return res;
};

const AllUsers = ({ loaderData }: Route.ComponentProps) => {
  const { users } = loaderData;

  return (
    <main className="all-users wrapper">
      <Header
        title="Manage Users"
        description="Filter, sort, and access detailed user profiles"
      />
      <GridComponent dataSource={users} gridLines="None">
        <ColumnsDirective>
          <ColumnDirective
            field="name" // prop name to use to get data from a datum in dataSource
            headerText="Name"
            width="200"
            textAlign="Left"
            template={(props: UserData) => (
              <div className="flex items-center gap-1.5 px-4">
                {/* TODO: Implement profile pics later */}
                {/* <img
                  src={props.imageUrl} alt="user" className="rounded-full size-8 aspect-square"
                /> */}
                <span>{props.name}</span>
              </div>
            )}
          />

          <ColumnDirective
            field="email"
            headerText="Email Address"
            width="200"
            textAlign="Left"
          />

          <ColumnDirective
            field="dateJoined"
            headerText="Date Joined"
            width="140"
            textAlign="Left"
            template={({ dateJoined: joinedAt }: UserData) =>
              formatDate(joinedAt)
            }
          />

          <ColumnDirective
            field="status"
            headerText="Type"
            width="100"
            textAlign="Left"
            template={({ roles }: UserData) => (
              <>
                {roles.map((role) => (
                  <article
                    className={cn(
                      "status-column",
                      role === ROLE_NORMALIZED_NAMES.User
                        ? "bg-success-50"
                        : "bg-light-300"
                    )}
                  >
                    <div
                      className={cn(
                        "size-1.5 rounded-full",
                        role === ROLE_NORMALIZED_NAMES.User
                          ? "bg-success-500"
                          : "bg-gray-500"
                      )}
                    />
                    <h3
                      className={cn(
                        "font-inter text-xs font-medium",
                        role === ROLE_NORMALIZED_NAMES.User
                          ? "text-success-700"
                          : "text-gray-500"
                      )}
                    >
                      {role.toLocaleLowerCase()}
                    </h3>
                  </article>
                ))}
              </>
            )}
          />
        </ColumnsDirective>
      </GridComponent>
    </main>
  );
};

export default AllUsers;
