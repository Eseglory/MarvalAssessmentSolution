import { useAuth } from "contexts/AuthContext";
import { useQuery, UseQueryOptions } from "react-query";
import { getAllUploadedRecords, viewPerson } from "services/person";
import { PersonI } from "types/client";

export const useGetAllUploadedRecords = (
  options?: UseQueryOptions<PersonI[]>
) => {
  const { user } = useAuth();

  return useQuery({
    queryKey: ["getAllUploadedRecords", user?.token],
    queryFn: () => getAllUploadedRecords(user?.token || ""),
    ...options,
  });
};

export const useGetSinglePerson = (
  id: string,
  options?: UseQueryOptions<PersonI>
) => {
  const { user } = useAuth();

  return useQuery({
    queryKey: ["viewPerson", user?.token],
    queryFn: () => viewPerson(id, user?.token || ""),
    ...options,
  });
};
