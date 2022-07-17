import { Button } from "components";
import { useAuth } from "contexts/AuthContext";
import { useForm } from "hooks/utils/useForm";
import React, { useState } from "react";
import { uploadCsv } from "services/person";
import "./uploadCsv.scss";

interface InitialStateI {
  file: File | null;
}

const initialState: InitialStateI = {
  file: null,
};

interface Props {
  handleUploadCsvClose: () => void;
  setIsUploadCsvLoading: React.Dispatch<React.SetStateAction<boolean>>;
  isUploadCsvLoading: boolean;
  setShowAlert: React.Dispatch<React.SetStateAction<boolean>>;
}

const UploadCsv: React.FC<Props> = ({
  handleUploadCsvClose,
  setIsUploadCsvLoading,
  setShowAlert,
  isUploadCsvLoading,
}) => {
  const { inputs, handleChange } = useForm<InitialStateI>(initialState);
  const { user } = useAuth();
  const [errs, setErrs] = useState("");

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setIsUploadCsvLoading(true);
    const payload = new FormData();
    payload.append("file", inputs.file!);

    try {
      const res = await uploadCsv(payload, user?.token || "");

      if (res) {
        handleUploadCsvClose();

        setShowAlert(true);
      }
      setIsUploadCsvLoading(false);
    } catch (error: any) {
      setIsUploadCsvLoading(false);

      setErrs(error?.response?.data);
    }
  };

  return (
    <form className="uploadCsv" onSubmit={handleSubmit}>
      <h3 className="uploadCsv__header">Upload CSV</h3>

      <input
        type="file"
        className="uploadCsv__input"
        accept=".csv"
        name="file"
        onChange={handleChange}
        id=""
      />
      {errs && <div className="uploadCsv__err">{errs}</div>}
      <div className="uploadCsv__btnDiv">
        <Button
          loading={isUploadCsvLoading}
          fullWidth
          type="submit"
          className="uploadCsv__btn"
        >
          Upload File
        </Button>
      </div>
    </form>
  );
};

export default UploadCsv;
