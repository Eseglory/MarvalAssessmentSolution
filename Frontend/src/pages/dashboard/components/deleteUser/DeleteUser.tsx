import { Button } from "components";
import React from "react";
import { PersonI } from "types/client";
import "./deleteUser.scss";

interface Props {
  isDeleteUserLoading: boolean;
  handleDeleteUser: (id: string) => Promise<void>;
  selectedUser: PersonI | undefined;
  handleDeleteUserClose: () => void;
  err: string;
}

const DeleteUser: React.FC<Props> = ({
  isDeleteUserLoading,
  handleDeleteUser,
  selectedUser,
  handleDeleteUserClose,
  err,
}) => {
  return (
    <div className="deleteUser">
      <h3 className="deleteUser__header">Delete User</h3>

      {err && <div className="deleteUser__err">{err}</div>}
      <div className="deleteUser__btnDiv">
        <Button
          className="deleteUser__btn"
          loading={isDeleteUserLoading}
          onClick={() =>
            handleDeleteUser(selectedUser?.identity.toString() || "")
          }
        >
          Yes
        </Button>
        <Button onClick={handleDeleteUserClose} className="deleteUser__btn">
          No
        </Button>
      </div>
    </div>
  );
};

export default DeleteUser;
