import { AxiosConfig } from "config/AxiosConfig";
import { PersonI, UserI } from "types/client";

export const getAllUploadedRecords = async (
  token: string
): Promise<PersonI[]> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.get<PersonI[]>(
    "/Person/get-all-uploaded-records",
    config
  );

  return res.data;
};

export const viewPerson = async (
  id: string,
  token: string
): Promise<PersonI> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.get<PersonI>(
    `/Person/get-person/${id}`,
    config
  );

  return res.data;
};

export const addPerson = async (
  payload: PersonI,
  token: string
): Promise<any> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.post<UserI>(
    "/Person/add-person",
    payload,
    config
  );

  return res.data;
};

export const updatePerson = async (
  payload: PersonI,
  token: string
): Promise<UserI> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.put<UserI>(
    "/Person/update-person",
    payload,
    config
  );

  return res.data;
};

export const deletePerson = async (id: string, token: string): Promise<any> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.delete(`/Person/delete-person/${id}`, config);

  return res.data;
};

export interface UploadCsvI {
  file: File;
}

export const uploadCsv = async (
  payload: FormData,
  token: string
): Promise<any> => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const res = await AxiosConfig.post(
    `/Person/upload-person-csv-file`,
    payload,
    config
  );

  return res.data;
};
